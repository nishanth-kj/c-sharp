[Setup]
AppName=SharpIB
AppVersion=1.0.0
DefaultDirName={autopf}\SharpIB
DefaultGroupName=SharpIB
OutputDir=release
OutputBaseFilename=SharpIB-Setup-win-x64
Compression=lzma2
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "release\desktop\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
