[Setup]
AppName=person-wpf-demo
AppVersion=1.0.0
DefaultDirName={pf}\person-wpf-demo
DefaultGroupName=person-wpf-demo
OutputBaseFilename=setup
OutputDir=.
Compression=lzma
SolidCompression=yes

[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\person-wpf-demo"; Filename: "{app}\person-wpf-demo.exe"

[Run]
Filename: "{app}\person-wpf-demo.exe"; Description: "Lancer person-wpf-demo"; Flags: nowait postinstall skipifsilent
