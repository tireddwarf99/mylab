# Лабораторна робота № 1 — Хмарні технології

Індивідуальний вебпроєкт Node.js для розгортання на Render.

Автор: Гирладжиу О.О., група ІТПм-25-1.
Викладач: Мінухін Сергій Володимирович.

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

## Render

У Render Dashboard створіть Blueprint і підключіть цей репозиторій.
Файл `render.yaml` розташований у корені та описує два сервіси:

| Сервіс | APP_VARIANT |
| --- | --- |
| cloud-lab1-baseline | baseline |
| cloud-lab1-modified | modified |

Команда підготовки: `npm install --omit=dev`.
Команда запуску: `npm start`.
Шлях перевірки стану: `/health`.

Сервер слухає `0.0.0.0` та використовує змінну `PORT`, яку задає Render.
Перед створенням перевірте доступність тарифу Free та назви сервісів.
Після розгортання відкрийте адреси обох сервісів і збережіть скриншоти для звіту.

## Перевірені маршрути

| Запит | Очікуваний результат |
| --- | --- |
| GET / | HTTP 200, сторінка обраної версії |
| GET /health | HTTP 200, JSON зі станом `ok` і версією |
| GET /api/info | HTTP 200, JSON із даними роботи |
| GET /missing | HTTP 404 |
| HEAD /health | HTTP 200 без тіла |
| POST /health | HTTP 405 |

Локально перевірено обидві версії, кнопку перевірки стану та відображення на мобільному екрані.
Хмарне розгортання поки не виконано.

Документація: https://render.com/docs/blueprint-spec та https://render.com/docs/free.
