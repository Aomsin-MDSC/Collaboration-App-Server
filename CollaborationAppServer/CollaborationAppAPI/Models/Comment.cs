using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Comment
    {
        [Key]
        public int Comment_id { get; set; }
        public required string Comment_text { get; set; }
        public int User_id { get; set; }
        public User User { get; set; }
    }
}
