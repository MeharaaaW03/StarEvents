using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StarEvents.Models;
using StarEvents.Services;

namespace StarEvents.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoDBContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(MongoDBContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Safe way to get events - won't crash if collection is empty
                var featuredEvents = await _context.Events
                    .Find(_ => true)
                    .Limit(3)
                    .ToListAsync();

                ViewBag.FeaturedEvents = featuredEvents;
            }
            catch (Exception ex)
            {
                // If there's an error (like no events yet), return empty list
                ViewBag.FeaturedEvents = new List<Event>();
                _logger.LogWarning("No events found or events collection not ready: {Message}", ex.Message);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}