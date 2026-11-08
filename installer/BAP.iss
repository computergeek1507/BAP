; Inno Setup script for The Brick Automation Project (BAP)
; Build locally:   iscc installer\BAP.iss
; Build in CI:     iscc /DMyAppVersion=1.7.0.0 /DBuildDir=..\out\Release installer\BAP.iss

#define MyAppName      "The Brick Automation Project"
#define MyAppShortName "BAP"
#define MyAppExeName   "LegoTrainProject.exe"
#define MyAppURL       "https://github.com/computergeek1507/BAP"

#ifndef BuildDir
  #define BuildDir "..\out\Release"
#endif

#ifndef MyAppVersion
  ; Fall back to the version stamped into the built exe
  #define MyAppVersion GetVersionNumbersString(AddBackslash(BuildDir) + MyAppExeName)
#endif

[Setup]
; Same UpgradeCode GUID as the old BAP_Setup.vdproj so the identity carries over
AppId={{DE1E831F-78AF-4560-BE8B-FC719FBA582A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher=BAP
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}/releases
DefaultDirName={autopf}\{#MyAppShortName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE
SetupIconFile=..\BAP.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
OutputDir=Output
OutputBaseFilename=BAP_Setup_{#MyAppVersion}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
; Per-user install by default (no UAC prompt); user can choose all-users instead
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
; Bluetooth LE via WinRT needs Windows 10 or later
MinVersion=10.0
; Self-contained win-x64 build
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
CloseApplications=yes
VersionInfoVersion={#MyAppVersion}
VersionInfoProductName={#MyAppName}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Dirs]
; The app saves connectionSettings.bap next to the exe, so the install
; folder must be writable even for an all-users install in Program Files.
Name: "{app}"; Permissions: users-modify

[Files]
; Self-contained .NET publish output (includes the runtime and its subfolders)
Source: "{#BuildDir}\*"; Excludes: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\BAP.ico";   DestDir: "{app}"; Flags: ignoreversion
Source: "..\LICENSE";   DestDir: "{app}"; DestName: "LICENSE.txt"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}";                Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\BAP.ico"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}";          Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\BAP.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: files; Name: "{app}\connectionSettings.bap"

[InstallDelete]
; Clear out files from older .NET Framework builds before installing
Type: files; Name: "{app}\LegoTrainProject.exe.config"
