FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
#install node and angular packages
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    npm install -g yarn@latest
#copy the solution and the projects
COPY ["BackPanel.sln", "./"]
# Copy all project files
COPY ["Presentation/BackPanel.WebApplication/BackPanel.WebApplication.csproj", "Presentation/BackPanel.WebApplication/"]
COPY ["Infrastructure/BackPanel.Persistence/BackPanel.Persistence.csproj", "Infrastructure/BackPanel.Persistence/"]
COPY ["Core/BackPanel.Application/BackPanel.Application.csproj", "Core/BackPanel.Application/"]
COPY ["Core/BackPanel.Domain/BackPanel.Domain.csproj", "Core/BackPanel.Domain/"]
COPY ["Infrastructure/BackPanel.FilesManager/BackPanel.FilesManager.csproj", "Infrastructure/BackPanel.FilesManager/"]
COPY ["Infrastructure/BackPanel.SMTP/BackPanel.SMTP.csproj", "Infrastructure/BackPanel.SMTP/"]
COPY ["Infrastructure/BackPanel.TranslationEditor/BackPanel.TranslationEditor.csproj", "Infrastructure/BackPanel.TranslationEditor/"]
COPY ["SourceGenerators/BackPanel.SourceGenerator/BackPanel.SourceGenerator.csproj", "SourceGenerators/BackPanel.SourceGenerator/"]
RUN dotnet restore
#copy everything else to host machine
COPY  . .
#install angular packages
RUN cd ./Presentation/BackPanel.WebApplication/ClientApp &&\
    npx yarn && \
    cd ./
#build the solution
RUN dotnet build -c Release -o /app/build
FROM build AS publish
RUN dotnet publish -c Release  -o /app/publish

#final stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS final
COPY --from=publish /app/publish /app
EXPOSE 80
EXPOSE 5000
EXPOSE 443
ENTRYPOINT ["dotnet", "BackPanel.WebApplication.dll"]
WORKDIR /app


