using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StarEvents.Services;

namespace StarEvents.Controllers
{
    public class TestController : Controller
    {
        private readonly MongoDBContext _context;

        public TestController(MongoDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // Test the MongoDB connection
                var database = _context.Database;
                var collections = database.ListCollectionNames().ToList();

                return Content($"✅ MongoDB Connection Successful! \n" +
                             $"Database: {database.DatabaseNamespace.DatabaseName} \n" +
                             $"Collections: {string.Join(", ", collections)}");
            }
            catch (Exception ex)
            {
                return Content($"❌ MongoDB Connection Failed: {ex.Message}");
            }
        }

        // ADD THIS NEW METHOD TO TEST USER COLLECTION
        public IActionResult TestUser()
        {
            try
            {
                // Test user collection access
                var userCount = _context.Users.CountDocuments(_ => true);
                return Content($"✅ User collection accessible! Total users: {userCount}");
            }
            catch (Exception ex)
            {
                return Content($"❌ User test failed: {ex.Message}");
            }
        }
    }
}