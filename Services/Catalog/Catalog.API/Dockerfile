FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY ApiGateways/nginx/id-local.crt /usr/local/share/ca-certificates/id-local.crt
RUN update-ca-certificates
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
#Todo: 2.26 Adjust sourcecode folder
#Chỉ copy csproj
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
COPY ["Services/Catalog/Catalog.Application/Catalog.Application.csproj", "Services/Catalog/Catalog.Application/"]
COPY ["Services/Catalog/Catalog.Core/Catalog.Core.csproj", "Services/Catalog/Catalog.Core/"]
COPY ["Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Services/Catalog/Catalog.Infrastructure/"]
COPY ["Infrastructure/Common.Logging/Common.Logging.csproj", "Infrastructure/Common.Logging/"]

#Restore thư viện và các csproj phụ thuộc kèm theo
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"

#Copy tất cả
COPY . .

#Build: Lấy nuget từ bộ nhớ cache, không sẵn sàng để chạy trên máy khác
#Test: Kiểm tra xem có build được hay không ?
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

#Publish: Sẵn sàng sử dụng để chạy, đã có đầy đủ thư viện
FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish

#Lấy từ Stage publish và chạy
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
