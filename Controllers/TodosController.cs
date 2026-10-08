using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
    public class TodosController : Controller
    {
        private readonly ILogger<TodosController> _logger;
        private readonly MyDatabaseContext _context;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "todos";

        public TodosController(MyDatabaseContext context, ILogger<TodosController> logger, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        // GET: Todos
        public async Task<IActionResult> Index()
        {
            return View(await BuildIndexViewModel());
        }

        // POST: Todos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Description", Prefix = "NewTodo")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(CacheKey);
                _logger.LogInformation("Task data changed; list cache invalidated.");

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index), await BuildIndexViewModel(todo));
        }

        // POST: Todos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();
                await _cache.RemoveAsync(CacheKey);
                _logger.LogInformation("Task data changed; list cache invalidated.");

            return RedirectToAction(nameof(Index));
        }

        private async Task<TodosIndexViewModel> BuildIndexViewModel(Todo? newTodo = null)
        {
            var cached = await _cache.GetStringAsync(CacheKey);
            List<Todo> todos;
            if (cached is null)
            {
                todos = await _context.Todo.AsNoTracking().ToListAsync();
                await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(todos),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
                _logger.LogInformation("Cache miss; loaded {Count} tasks from database.", todos.Count);
            }
            else
            {
                todos = JsonSerializer.Deserialize<List<Todo>>(cached) ?? new();
                _logger.LogInformation("Cache hit; returned {Count} tasks.", todos.Count);
            }
            return new TodosIndexViewModel
            {
                NewTodo = newTodo ?? new Todo(),
                Todos = todos
            };
        }
    }
}
