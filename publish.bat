@echo off
REM Publishes a single executable into publish directory.
REM
REM In order to generate debug information remove the DebugType option.
REM
REM In order to generate smaller executable, but which will relay on existing
REM installation of .NET Core, remove the PublishSingleFile option.
@echo on

rmdir /S /Q publish
dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:DebugType=None --output ./publish CLI\CLI.csproj

