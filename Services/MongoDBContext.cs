using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StarEvents.Models;

namespace StarEvents.Services
{
    public class MongoDBContext
    {
        public IMongoDatabase Database { get; }

        public MongoDBContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);
        }

        // Add collections for all your models here:
        public IMongoCollection<User> Users => Database.GetCollection<User>("Users");

        // ADD THIS EVENT COLLECTION:
        public IMongoCollection<Event> Events => Database.GetCollection<Event>("Events");

        // You can add these later as you create the models:
        // public IMongoCollection<Venue> Venues => Database.GetCollection<Venue>("Venues");
        // public IMongoCollection<Ticket> Tickets => Database.GetCollection<Ticket>("Tickets");
        // public IMongoCollection<Booking> Bookings => Database.GetCollection<Booking>("Bookings");
    }
}