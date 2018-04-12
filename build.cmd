@echo off

echo Cleaning ...
del %cd%\bin /F /Q
dotnet clean src\Trail.BlazorRedux --configuration Release
dotnet clean src\Trail --configuration Release

echo Building ...
dotnet build src\Trail --configuration Release -v n /p:SourceLinkCreate=true
dotnet build src\Trail.BlazorRedux --configuration Release -v n /p:SourceLinkCreate=true

echo Testing ...
dotnet test test\Trail.Tests

echo Packing ...
dotnet pack src\Trail --configuration Release -o %cd%\bin
dotnet pack src\Trail.BlazorRedux --configuration Release -o %cd%\bin

echo Completed successfully!
