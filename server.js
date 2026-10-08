const http = require('node:http');
const fs = require('node:fs');
const path = require('node:path');

const variant = process.env.APP_VARIANT || 'modified';
if (!['baseline', 'modified'].includes(variant)) throw new Error('APP_VARIANT must be baseline or modified');
const port = Number(process.env.PORT || 3000);
const page = fs.readFileSync(path.join(__dirname, 'public', `${variant}.html`));
const server = http.createServer((req, res) => {
  const pathname = new URL(req.url, 'http://localhost').pathname;
  res.setHeader('X-Content-Type-Options', 'nosniff');
  if (req.method !== 'GET' && req.method !== 'HEAD') {
    res.writeHead(405, { Allow: 'GET, HEAD' }); return res.end();
  }
  let body;
  if (pathname === '/') {
    res.setHeader('Content-Type', 'text/html; charset=utf-8'); body = page;
  } else if (pathname === '/health') {
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    body = JSON.stringify({status:'ok',variant,service:'cloud-lab1'});
  } else if (pathname === '/api/info') {
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    body = JSON.stringify({author:'Гирладжиу О.О.',group:'ІТПм-25-1',laboratory:1,variant,runtime:process.version});
  } else {
    res.writeHead(404, {'Content-Type':'text/plain; charset=utf-8'});
    return res.end(req.method === 'HEAD' ? undefined : 'Сторінку не знайдено');
  }
  res.writeHead(200);
  res.end(req.method === 'HEAD' ? undefined : body);
});
server.listen(port, '0.0.0.0', () => console.log(`cloud-lab1: ${variant} http://localhost:${port}`));
for (const signal of ['SIGINT','SIGTERM']) process.on(signal, () => server.close(() => process.exit(0)));
