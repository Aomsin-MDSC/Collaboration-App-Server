using System.ComponentModel.DataAnnotations;
using Azure;

namespace CollaborationAppAPI.Models
{
    public class Project
    {
        [Key]
        public int Project_id { get; set; }
        public required string Project_name { get; set; }

        // Foreign Keys
        public int User_id { get; set; }
        public User User { get; set; }


        public Member Member { get; set; }

        public int? Tag_id { get; set; }
        public Tag Tag { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public Announce Announce { get; set; }
    }
}
