using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace LegoTrainProject
{
	/// <summary>
	/// Reads and writes BAP files (.ltp projects, connectionSettings.bap) as JSON.
	///
	/// The JSON contract deliberately mirrors what BinaryFormatter used to store, so the
	/// existing [Serializable] / [NonSerialized] markup keeps deciding what gets saved:
	///   - every instance field of a [Serializable] BAP type is saved (public or private),
	///     except [NonSerialized] fields and delegates (events)
	///   - objects are created without running constructors, like BinaryFormatter did
	///   - shared references and cycles are kept ($id / $ref), e.g. a Hub that is both in
	///     RegisteredTrains and a Section's CurrentHub
	///   - Hub subclasses are tagged with "$type"
	///
	/// Files written by older versions (BinaryFormatter) are still read, and are saved
	/// as JSON the next time they are saved.
	/// </summary>
	public static class ProjectSerializer
	{
		private const string FormatName = "BAP";
		private const int FormatVersion = 1;

		private static readonly Assembly BapAssembly = typeof(TrainProject).Assembly;

		public static readonly JsonSerializerOptions Options = CreateOptions();

		/// <summary>
		/// Envelope written at the top of every file so future versions can migrate.
		/// </summary>
		private sealed class FileEnvelope<T>
		{
			public string Format { get; set; }
			public int Version { get; set; }
			public string Type { get; set; }
			public T Data { get; set; }
		}

		public static void Save<T>(T value, string path)
		{
			var envelope = new FileEnvelope<T>
			{
				Format = FormatName,
				Version = FormatVersion,
				Type = typeof(T).Name,
				Data = value
			};

			// Write to a temp file first so a crash mid-save can't destroy the project
			string temp = path + ".tmp";
			using (var stream = new FileStream(temp, FileMode.Create, FileAccess.Write))
				JsonSerializer.Serialize(stream, envelope, Options);

			File.Move(temp, path, overwrite: true);
		}

		public static T Load<T>(string path, out bool wasLegacyFormat)
		{
			using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				wasLegacyFormat = !LooksLikeJson(stream);
				stream.Position = 0;

				if (wasLegacyFormat)
					return LoadLegacy<T>(stream);

				var envelope = JsonSerializer.Deserialize<FileEnvelope<T>>(stream, Options);

				if (envelope == null || envelope.Format != FormatName)
					throw new InvalidDataException("This is not a BAP file.");
				if (envelope.Version > FormatVersion)
					throw new InvalidDataException($"This file was saved by a newer version of BAP (format {envelope.Version}). Please update BAP.");

				return envelope.Data;
			}
		}

		private static bool LooksLikeJson(Stream stream)
		{
			// Skip a UTF-8 BOM and whitespace; JSON starts with '{', BinaryFormatter with 0x00
			int b;
			while ((b = stream.ReadByte()) != -1)
			{
				if (b == 0xEF || b == 0xBB || b == 0xBF || char.IsWhiteSpace((char)b))
					continue;
				return b == '{';
			}
			return false;
		}

#pragma warning disable SYSLIB0011 // BinaryFormatter: only used to import files from BAP 1.7 and older
		private static T LoadLegacy<T>(Stream stream)
		{
			var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
			{
				// Only BAP's own types and the framework types they contain may be created
				Binder = new LegacyBinder()
			};
			return (T)formatter.Deserialize(stream);
		}

		private sealed class LegacyBinder : System.Runtime.Serialization.SerializationBinder
		{
			private static readonly string[] AllowedFrameworkTypes =
			{
				"System.Collections.Generic.List`1",
				"System.Collections.Generic.ObjectEqualityComparer`1",
				"System.Collections.Generic.GenericEqualityComparer`1",
				"System.Drawing.Color",
				"System.Drawing.KnownColor",
			};

			public override Type BindToType(string assemblyName, string typeName)
			{
				string baseName = typeName.Split('[')[0];

				if (assemblyName.StartsWith(BapAssembly.GetName().Name + ",", StringComparison.Ordinal))
					return BapAssembly.GetType(typeName, throwOnError: true);

				if (AllowedFrameworkTypes.Contains(baseName) || IsPrimitiveName(baseName))
				{
					// Resolve via the type's own name so .NET Framework assembly names map to .NET
					Type t = Type.GetType($"{typeName}, {assemblyName}", throwOnError: false)
						?? Type.GetType(typeName, throwOnError: false)
						?? (baseName == "System.Drawing.Color" ? typeof(Color) : null)
						?? (baseName == "System.Drawing.KnownColor" ? typeof(KnownColor) : null);
					if (t != null)
						return t;
				}

				throw new InvalidDataException($"Unexpected type in old project file: {typeName}");
			}

			private static bool IsPrimitiveName(string name) =>
				name.StartsWith("System.", StringComparison.Ordinal) &&
				Type.GetType(name) is Type t && (t.IsPrimitive || t == typeof(string) || t.IsEnum || t == typeof(decimal));
		}
#pragma warning restore SYSLIB0011

		//////////////////////////////////////////
		//// JSON contract
		//////////////////////////////////////////

		private static JsonSerializerOptions CreateOptions()
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				ReferenceHandler = ReferenceHandler.Preserve,
				// Sections link to each other, so long tracks nest deeply
				MaxDepth = 4096,
				NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
				TypeInfoResolver = new DefaultJsonTypeInfoResolver
				{
					Modifiers = { UseSerializableFields }
				}
			};
			options.Converters.Add(new JsonStringEnumConverter());
			options.Converters.Add(new ColorConverter());
			return options;
		}

		private static bool IsBapSerializable(Type t) =>
			t.Assembly == BapAssembly && (t.Attributes & TypeAttributes.Serializable) != 0;

		private static void UseSerializableFields(JsonTypeInfo info)
		{
			Type type = info.Type;
			if (info.Kind != JsonTypeInfoKind.Object || !IsBapSerializable(type))
				return;

			info.Properties.Clear();
			if (!type.IsAbstract)
				info.CreateObject = () => RuntimeHelpers.GetUninitializedObject(type);

			// Base class fields first; a field hidden by a subclass gets its class as prefix
			var chain = new List<Type>();
			for (Type c = type; c != null && c != typeof(object); c = c.BaseType)
				chain.Insert(0, c);

			var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (Type c in chain)
			{
				foreach (FieldInfo field in c.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
				{
					if ((field.Attributes & FieldAttributes.NotSerialized) != 0)
						continue;
					if (typeof(Delegate).IsAssignableFrom(field.FieldType))
						continue;

					string name = CleanFieldName(field.Name);
					if (!usedNames.Add(name))
						name = c.Name + "." + name;

					JsonPropertyInfo prop = info.CreateJsonPropertyInfo(field.FieldType, name);
					FieldInfo f = field;
					prop.Get = obj => f.GetValue(obj);
					prop.Set = (obj, value) => f.SetValue(obj, value);
					info.Properties.Add(prop);
				}
			}

			// Hub (and BuWizzHub) are saved through base-typed fields and lists
			var derived = BapAssembly.GetTypes()
				.Where(t => t != type && !t.IsAbstract && type.IsAssignableFrom(t) && IsBapSerializable(t))
				.OrderBy(t => t.Name)
				.ToList();

			if (derived.Count > 0)
			{
				info.PolymorphismOptions = new JsonPolymorphismOptions
				{
					TypeDiscriminatorPropertyName = "$type",
					UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
				};
				foreach (Type t in derived)
					info.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(t, t.Name));
			}
		}

		/// <summary>
		/// "&lt;Name&gt;k__BackingField" (auto-property) -> "Name"
		/// </summary>
		private static string CleanFieldName(string name)
		{
			if (name.StartsWith("<", StringComparison.Ordinal))
			{
				int end = name.IndexOf('>');
				if (end > 1)
					return name.Substring(1, end - 1);
			}
			return name;
		}

		/// <summary>
		/// Colors are saved by name when they have one, otherwise as #AARRGGBB
		/// </summary>
		private sealed class ColorConverter : JsonConverter<Color>
		{
			public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				string s = reader.GetString();
				if (string.IsNullOrEmpty(s))
					return Color.Empty;
				if (s[0] == '#' && uint.TryParse(s.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint argb))
					return Color.FromArgb(unchecked((int)argb));
				return Color.FromName(s);
			}

			public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
			{
				if (value.IsEmpty)
					writer.WriteStringValue("");
				else if (value.IsNamedColor)
					writer.WriteStringValue(value.Name);
				else
					writer.WriteStringValue("#" + value.ToArgb().ToString("X8", CultureInfo.InvariantCulture));
			}
		}
	}
}
