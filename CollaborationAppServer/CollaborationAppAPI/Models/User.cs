using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class User
    {
        [Key]
        public int User_id { get; set; }
        public required string User_name { get; set; }
        public required string User_password { get; set; }
        public string? User_token { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
