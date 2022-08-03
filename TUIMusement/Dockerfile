FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /TUIMusement

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /TUIMusement
COPY --from=build-env /TUIMusement/out .
ENTRYPOINT ["dotnet", "TUIMusement.dll"]
