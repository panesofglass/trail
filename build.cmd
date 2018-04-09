@echo off

echo Cleaning ...
del %cd%\bin /F /Q
dotnet clean src\Trail.BlazorRedux --configuration Release
dotnet clean src\Trail.Flatware --configuration Release
dotnet clean src\Trail --configuration Release

echo Building ...
dotnet restore
dotnet build src\Trail --configuration Release
dotnet build src\Trail.Flatware --configuration Release
dotnet build src\Trail.BlazorRedux --configuration Release

echo Testing ...
dotnet test test\Trail.Tests

echo Packing ...
dotnet pack src\Trail --configuration Release -o %cd%\bin
dotnet pack src\Trail.Flatware --configuration Release -o %cd%\bin
dotnet pack src\Trail.BlazorRedux --configuration Release -o %cd%\bin

echo Completed successfully!
