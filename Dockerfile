# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore as distinct layers
COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/out .

# Install dotnet-ef tool
RUN echo "Installing ef globally"
RUN echo "=========================="
RUN dotnet tool install --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

# Verify installation
RUN echo "PATH is: $PATH" && \
    ls /root/.dotnet/tools && \
    dotnet ef --version

# Copy source files needed for migrations
COPY --from=build /app/Onyx.Product.Infrastructure /app/Onyx.Product.Infrastructure
COPY --from=build /app/Onyx.Products.Domain /app/Onyx.Products.Domain
COPY --from=build /app/Onyx.ProductsService /app/Onyx.ProductsService
COPY --from=build /app/Products /app/Products

# Install mssql-tools and dependencies
RUN apt-get update && \
    apt-get install -y curl apt-transport-https gnupg2 && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql18 mssql-tools18 && \
    echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV PATH="$PATH:/opt/mssql-tools18/bin:/root/.dotnet/tools"

# Command to run migrations and start the application
CMD /bin/bash -c '\
    echo "Waiting for SQL Server to start..."; \
    until sqlcmd -S tcp:sqlserver,1433 -U sa -P Onyx2Sqlserver£pwd -C -Q "SELECT 1" > /dev/null 2>&1; do \
        echo "SQL Server is starting up..."; \
        sleep 10; \
    done; \
    echo "Applying database migrations..."; \
    dotnet ef database update --project Onyx.Product.Infrastructure/Onyx.Product.Infrastructure.csproj --startup-project Products/Onyx.ProductsApi.csproj; \
    echo "Starting the product API application..."; \
    exec dotnet Onyx.ProductsApi.dll'