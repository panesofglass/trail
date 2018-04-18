#!/bin/bash

pwd="`pwd`"

echo Building Trail ...
mkdir bin
dotnet build src/Trail --configuration Release -v n

echo Testing Trail ...
dotnet test test/Trail.Tests

echo Packing Trail ...
dotnet pack src/Trail --configuration Release -o $pwd/bin

echo Building Trail.BlazorRedux ...
dotnet build src/Trail.BlazorRedux --configuration Release -v n

echo Packing Trail.BlazorRedux ...
dotnet pack src/Trail.BlazorRedux --configuration Release -o $pwd/bin

echo Completed successfully!
