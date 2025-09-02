using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StarEvents.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [Required]
        [BsonElement("category")]
        public string Category { get; set; } = null!;

        [BsonElement("dateTime")]
        public DateTime DateTime { get; set; }

        [BsonElement("venue")]
        public string Venue { get; set; } = null!;

        [BsonElement("ticketPrice")]
        public decimal TicketPrice { get; set; }

        [BsonElement("totalTickets")]
        public int TotalTickets { get; set; }

        [BsonElement("ticketsSold")]
        public int TicketsSold { get; set; } = 0;

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; } = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80";

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("organizerId")]
        public string OrganizerId { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}