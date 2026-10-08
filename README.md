# Лабораторна робота № 2 — Хмарні технології

ASP.NET Core MVC (.NET 8), Entity Framework Core 8, Azure SQL Database та Redis.
Проєкт адаптовано з Azure-Samples/msdocs-app-service-sqldb-dotnetcore.
Ліцензія оригіналу збережена в LICENSE.md.

## Локальний запуск

Потрібен .NET 8 SDK. У PowerShell:

```powershell
$env:ASPNETCORE_ENVIRONMENT='Development'
dotnet run --urls http://localhost:5082
```

Локально використовується SQLite й кеш у пам’яті. Production використовує Azure SQL та Redis.

## Параметри Azure App Service

Рядки підключення зберігаються в Azure, а не в GitHub:

- AZURE_SQL_CONNECTIONSTRING — Azure SQL (підтримується також Connection string з цією назвою).
- AZURE_REDIS_CONNECTIONSTRING — Redis.
- ASPNETCORE_ENVIRONMENT=Production.

## Міграція

GitHub Actions формує самодостатній Linux-пакет migrationsbundle.
У SSH App Service із каталогу /home/site/wwwroot:

```bash
chmod +x migrationsbundle
./migrationsbundle
```

## GitHub Actions

.github/workflows/azure-lab2.yml збирає .NET 8, формує міграцію та публікує через OIDC.
Потрібні variable AZURE_WEBAPP_NAME та secrets AZURE_CLIENT_ID, AZURE_TENANT_ID, AZURE_SUBSCRIPTION_ID.
Workflow запускається вручну. Deployment Center може створити власний workflow.

## Індивідуальне завдання

Microsoft.AspNetCore має LogLevel Information.
Контролер журналює читання БД, cache hit, cache miss і скидання кешу.
Потоковий перегляд: App Service logs / Log stream.
