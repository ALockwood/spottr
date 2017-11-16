#!/bin/bash
PUB_DIR="publish"

rm -rf MobileAppService/${PUB_DIR}

dotnet restore MobileAppService/spottr.MobileAppService.csproj -r linux-x64
dotnet publish MobileAppService/spottr.MobileAppService.csproj -c Release -r linux-x64 -o ${PUB_DIR}

docker build -t spottr-api:latest . 

#Run with:
#docker run -d -p 80:80 -e "ASPNETCORE_URLS=http://0.0.0.0:80" -e "HACK_PASSWORD=the_pwd" -e"HACK_SERVER=the_ip" --name hacky spottr-api