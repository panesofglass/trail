#!/bin/bash

pwd="`pwd`"

echo Building ...
dotnet build src/Trail --configuration Release -v n
dotnet build src/Trail.BlazorRedux --configuration Release -v n

echo Testing ...
dotnet test test/Trail.Tests

echo Packing ...
dotnet pack src/Trail --configuration Release -o $pwd/bin
dotnet pack src/Trail.BlazorRedux --configuration Release -o $pwd/bin

echo Completed successfully!
