@ECHO OFF

IF "%1"=="" (
CALL build.bat Release
SET build=%errorlevel%
EXIT /b %build%
)

IF NOT EXIST .\packages\nuget\nuget.exe (
IF NOT EXIST .\packages\nuget\ MKDIR .\packages\nuget
ECHO ^(New-Object System.Net.WebClient^).DownloadFile('https://dist.nuget.org/win-x86-commandline/v3.4.4/NuGet.exe', '.\\packages\\nuget\\nuget.exe'^) > .\packages\nuget\nuget.ps1
PowerShell.exe -ExecutionPolicy Bypass -File .\packages\nuget\nuget.ps1
)

.\packages\nuget\nuget.exe restore
IF ERRORLEVEL 1 GOTO ERROR

"C:\Program Files (x86)\MSBuild\14.0\Bin\Msbuild.exe" /t:Build .\Svenkle.TwoPly\Svenkle.TwoPly.csproj /p:VisualStudioVersion=14.0;Configuration=%1
IF ERRORLEVEL 1 GOTO ERROR

IF NOT EXIST .\build MKDIR .\build
IF ERRORLEVEL 1 GOTO ERROR

.\packages\nuget\nuget.exe pack .\Svenkle.TwoPly\Svenkle.TwoPly.nuspec -Prop Configuration=Release -Verbosity normal -OutputDirectory .\build
IF ERRORLEVEL 1 GOTO ERROR

EXIT /b 0

:ERROR
ECHO Build Failed!
EXIT /b -1