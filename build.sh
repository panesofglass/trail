#!/bin/bash

pwd="`pwd`"

echo Cleaning ...
rm -rf $pwd/bin
dotnet clean src/Trail.BlazorRedux --configuration Release
dotnet clean src/Trail.Flatware --configuration Release
dotnet clean src/Trail --configuration Release

echo Building ...
dotnet restore
dotnet build src/Trail --configuration Release
dotnet build src/Trail.Flatware --configuration Release
dotnet build src/Trail.BlazorRedux --configuration Release

echo Testing ...
dotnet test test/Trail.Tests

echo Packing ...
dotnet pack src/Trail --configuration Release -o $pwd/bin
dotnet pack src/Trail.Flatware --configuration Release -o $pwd/bin
dotnet pack src/Trail.BlazorRedux --configuration Release -o $pwd/bin

echo Completed successfully!
