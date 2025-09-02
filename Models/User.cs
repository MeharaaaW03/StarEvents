using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StarEvents.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        // ADD CONFIRM PASSWORD FIELD (Not stored in MongoDB)
        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("PasswordHash", ErrorMessage = "Passwords do not match")]
        [BsonIgnore] // This tells MongoDB to ignore this property
        public string ConfirmPassword { get; set; } = null!;

        // UPDATE ROLE PROPERTY WITH OPTIONS
        [Required(ErrorMessage = "Please select a role")]
        [BsonElement("role")]
        public string Role { get; set; } = "Customer"; // Default to Customer

        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("profile")]
        public UserProfile? Profile { get; set; }
    }

    public class UserProfile
    {
        [BsonElement("firstName")]
        public string? FirstName { get; set; }

        [BsonElement("lastName")]
        public string? LastName { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }
    }
}