#!/bin/bash
PUB_DIR="publish"

rm -rf MobileAppService/${PUB_DIR}

dotnet restore MobileAppService/spottr.MobileAppService.csproj -r linux-x64
dotnet publish MobileAppService/spottr.MobileAppService.csproj -c Release -r linux-x64 -o ${PUB_DIR}

docker build -t spottr-api:latest . 
