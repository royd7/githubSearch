# Stage 1: Build the Angular application
FROM node:alpine AS frontend-build
#ARG NODE_EXTRA_CA_CERTS=/etc/ssl/certs/ca-certificates.crt

EXPOSE 80
EXPOSE 4200

WORKDIR /app/client
COPY client/githubSearch .

RUN npm config set registry http://registry.npmjs.org/ && npm config set strict-ssl false
#RUN npm cache clean --force
RUN npm install --verbose
RUN npm run build

# Stage 2: Build and publish the ASP.NET Core application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS backend-build
WORKDIR /app/server
COPY server/githubSearch/githubSearch.csproj .
RUN dotnet restore

COPY . .
WORKDIR /app/server
RUN dotnet build -c Release -o out

# Stage 3: Publish the ASP.NET Core application
FROM backend-build AS backend-publish
WORKDIR /app/server
RUN dotnet publish -c Release -o out

# Stage 4: Create the final image combining ASP.NET Core and Angular
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=backend-publish /app/server/out .
COPY --from=frontend-build /app/client/dist ./wwwroot
EXPOSE 80
ENTRYPOINT ["dotnet", "githubSearch.dll"]
