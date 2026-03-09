[Setup]
AppName=SharpIB
AppVersion=1.0.0
DefaultDirName={autopf}\SharpIB
DefaultGroupName=SharpIB
OutputDir={#OutputDir}
OutputBaseFilename=SharpIB-Setup-win-x64
Compression=lzma2
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
