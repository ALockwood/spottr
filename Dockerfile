FROM microsoft/aspnetcore:latest
WORKDIR /hackathon
COPY MobileAppService/publish/ ./
EXPOSE 80
ENTRYPOINT [ "dotnet", "spottr.MobileAppService.dll" ]
