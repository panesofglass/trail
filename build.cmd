@echo off

echo Cleaning ...
del %cd%\bin /F /Q
dotnet clean src\Trail.BlazorRedux --configuration Release
dotnet clean src\Trail --configuration Release

echo Building Trail ...
mkdir %cd%\bin
dotnet build src\Trail --configuration Release -v n

echo Testing Trail ...
dotnet test test\Trail.Tests

echo Packing Trail ...
dotnet pack src\Trail --configuration Release -o %cd%\bin

echo Building Trail.BlazorRedux ...
dotnet build src\Trail.BlazorRedux --configuration Release -v n

echo Packing Trail.BlazorRedux ...
dotnet pack src\Trail.BlazorRedux --configuration Release -o %cd%\bin

echo Completed successfully!
