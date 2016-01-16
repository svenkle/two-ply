@ECHO OFF

:: Use Release build configuration unless specified
IF "%1"=="" (
CALL build.bat Release
SET build=%errorlevel%
EXIT /b %build%
)

:: Download NuGet if its not already there
IF NOT EXIST .\packages\nuget\nuget.exe (
IF NOT EXIST .\packages\nuget\ MKDIR .\packages\nuget
ECHO ^(New-Object System.Net.WebClient^).DownloadFile('http://dist.nuget.org/win-x86-commandline/latest/nuget.exe', '.\\packages\\nuget\\nuget.exe'^) > .\nuget.ps1
PowerShell.exe -ExecutionPolicy Bypass -File .\nuget.ps1
)

:: Restore all packages
.\packages\nuget\nuget.exe restore

:: Build Two Ply
"C:\Program Files (x86)\MSBuild\14.0\Bin\Msbuild.exe" /t:Build .\Svenkle.TwoPly\Svenkle.TwoPly.csproj /p:VisualStudioVersion=14.0;Configuration=%1

:: Package Two Ply
IF NOT EXIST .\build MKDIR .\build
.\packages\nuget\nuget.exe pack .\Svenkle.TwoPly\Svenkle.TwoPly.nuspec -Prop Configuration=Release -Verbosity normal -OutputDirectory .\build

:: Clean up
IF EXIST .\nuget.ps1 DEL .\nuget.ps1

EXIT /b 0