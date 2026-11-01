using System.ComponentModel.DataAnnotations;

namespace CommentApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? EditedAt { get; set; }

        // Foreign key to the user who wrote it
        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        // Optional: which "post" or "thread" this comment belongs to.
        // Useful if you want more than one comment section in the app.
        public string ThreadKey { get; set; } = "default";
    }
}