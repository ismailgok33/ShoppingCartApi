FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ["CicekSepetiTask/CicekSepetiTask.csproj", "CicekSepetiTask/"]
COPY CicekSepetiTask/Setup.sh Setup.sh

RUN dotnet tool install --global dotnet-ef

RUN dotnet restore "CicekSepetiTask/CicekSepetiTask.csproj"
COPY . .
WORKDIR "/src/CicekSepetiTask"

RUN /root/.dotnet/tools/dotnet-ef migrations add InitialMigrationsss

RUN chmod +x ./Setup.sh
CMD /bin/bash ./Setup.sh