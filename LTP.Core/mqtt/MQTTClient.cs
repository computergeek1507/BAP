using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegoTrainProject
{
	public class MQTTClient
	{

		/// <summary>
		/// The managed subscriber client.
		/// </summary>
		private IManagedMqttClient managedMqttClientSubscriber;

		public MQTTClient() {




		}

		public async void ConnectToServer(string clientID, string server, int port)
		{
			var mqttFactory = new MqttFactory();

			var tlsOptions = new MqttClientTlsOptions
			{
				UseTls = false,
				IgnoreCertificateChainErrors = true,
				IgnoreCertificateRevocationErrors = true,
				AllowUntrustedCertificates = true
			};

			var options = new MqttClientOptions
			{
				ClientId = clientID,
				ProtocolVersion = MqttProtocolVersion.V311,
				ChannelOptions = new MqttClientTcpOptions
				{
					Server = server,
					Port = port,
					TlsOptions = tlsOptions
				}
			};

			if (options.ChannelOptions == null)
			{
				throw new InvalidOperationException();
			}

			//options.Credentials = new MqttClientCredentials("username", Encoding.UTF8.GetBytes("password"));
			options.CleanSession = true;
			options.KeepAlivePeriod = TimeSpan.FromSeconds(5);

			managedMqttClientSubscriber = mqttFactory.CreateManagedMqttClient();
			managedMqttClientSubscriber.ConnectedAsync += OnSubscriberConnected;
			managedMqttClientSubscriber.DisconnectedAsync += OnSubscriberDisconnected;
			managedMqttClientSubscriber.ApplicationMessageReceivedAsync += this.OnSubscriberMessageReceived;

			await managedMqttClientSubscriber.StartAsync(
				new ManagedMqttClientOptions
				{
					ClientOptions = options
				});


		}

		private Task OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs x)
		{
			var item = $"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel}";
			//this.BeginInvoke((MethodInvoker)delegate { this.TextBoxSubscriber.Text = item + Environment.NewLine + this.TextBoxSubscriber.Text; });
			return Task.CompletedTask;
		}

		private static Task OnPublisherConnected(MqttClientConnectedEventArgs _)
		{
			//MessageBox.Show("Publisher Connected", "ConnectHandler", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Handles the publisher disconnected event.
		/// </summary>
		private static Task OnPublisherDisconnected(MqttClientDisconnectedEventArgs _)
		{
			//MessageBox.Show("Publisher Disconnected", "ConnectHandler", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Handles the subscriber connected event.
		/// </summary>
		private static Task OnSubscriberConnected(MqttClientConnectedEventArgs _)
		{
			//MessageBox.Show("Subscriber Connected", "ConnectHandler", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Handles the subscriber disconnected event.
		/// </summary>
		private static Task OnSubscriberDisconnected(MqttClientDisconnectedEventArgs _)
		{
			//MessageBox.Show("Subscriber Disconnected", "ConnectHandler", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return Task.CompletedTask;
		}
	}
}
