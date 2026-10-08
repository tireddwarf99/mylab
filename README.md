# Лабораторна робота № 1 — Хмарні технології

Індивідуальний вебпроєкт Node.js для розгортання на Render.


## Вимоги

Node.js 22 або новіший. Зовнішніх бібліотек немає.

## Локальний запуск

У PowerShell із кореня проєкту запустіть початкову версію:

```powershell
$env:APP_VARIANT='baseline'
$env:PORT='3000'
node server.js
```

Адреса: http://localhost:3000.

В іншому вікні PowerShell запустіть змінену версію:

```powershell
$env:APP_VARIANT='modified'
$env:PORT='3001'
node server.js
```

Адреса: http://localhost:3001. Кнопка «Перевірити сервер» викликає `/health`.
Для зупинки натисніть Ctrl+C у відповідному вікні.

## Структура

- `server.js` — HTTP-сервер і JSON API.
- `public/baseline.html` — початкова сторінка.
- `public/modified.html` — персоналізована сторінка.
- `package.json` — параметри проєкту та команда `npm start`.
- `render.yaml` — конфігурація двох вебсервісів Render.

## Перевірені маршрути

| Запит | Очікуваний результат |
| --- | --- |
| GET / | HTTP 200, сторінка обраної версії |
| GET /health | HTTP 200, JSON зі станом `ok` і версією |
| GET /api/info | HTTP 200, JSON із даними роботи |
| GET /missing | HTTP 404 |
| HEAD /health | HTTP 200 без тіла |
| POST /health | HTTP 405 |

