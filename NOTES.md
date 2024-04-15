### Install packages

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.json
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### Install EF Migration Tool

```bash
dotnet tool install --global dotnet-ef
```

### Create migration

```bash
dotnet ef migrations add InitialCreate
```

### Apply migrations to database

```bash
dotnet ef database update
```