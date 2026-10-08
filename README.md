# Лабораторна робота № 2 — Хмарні технології

ASP.NET Core MVC (.NET 8), Entity Framework Core 8, Azure SQL Database та Azure Managed Redis.
Проєкт адаптовано з Azure-Samples/msdocs-app-service-sqldb-dotnetcore.
Ліцензія оригіналу збережена в LICENSE.md.

## Локальний запуск

Потрібен .NET 8 SDK. У PowerShell:

```powershell
$env:ASPNETCORE_ENVIRONMENT='Development'
dotnet run --urls http://localhost:5082
```

Локально використовується SQLite й кеш у пам’яті. Production використовує Azure SQL та Azure Managed Redis.

## Параметри Azure App Service

- AZURE_SQL_CONNECTIONSTRING — рядок підключення Azure SQL у Connection strings.
- AZURE_REDIS_HOST — адреса Azure Managed Redis; порт 10000, TLS, авторизація через системну керовану ідентичність вебдодатка.
- ASPNETCORE_ENVIRONMENT=Production.

Паролі й токени в репозиторії не зберігаються. Публічний доступ до Redis вимкнений; мережеве підключення проходить через Private Endpoint.

## Міграція та розгортання

GitHub Actions збирає .NET 8, формує самодостатній Linux-пакет migrationsbundle й публікує застосунок в Azure через OIDC.
Workflow запускається після змін у гілці lab2-azure. Ідентифікатори дозволеної Azure-ідентичності задано в workflow; секрети для входу не потрібні.
Команда запуску App Service застосовує migrationsbundle перед запуском DotNetCoreSqlDb.dll.

## Індивідуальне завдання

Microsoft.AspNetCore має LogLevel Information.
Контролер журналює читання БД, cache hit, cache miss і скидання кешу.
Потоковий перегляд: App Service logs / Log stream.
