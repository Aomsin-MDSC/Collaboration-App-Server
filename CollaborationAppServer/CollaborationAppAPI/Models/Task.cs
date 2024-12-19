using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Task
    {
        [Key]
        public int Task_id { get; set; }
        public required string Task_name { get; set; }
        public required string Task_detail { get; set; }
        public required DateTime Task_end { get; set; }
        public required string Task_color { get; set; }
        public required string Task_status { get; set; }
        public int? Project_id { get; set; }
        public Project? Project { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public int? Tag_id { get; set; }
        public Tag? Tag { get; set; }
        public int? User_id { get; set; }
        public User? User { get; set; }


    }
}
