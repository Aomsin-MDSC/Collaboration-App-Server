using System.ComponentModel.DataAnnotations;
using Azure;

namespace CollaborationAppAPI.Models
{
    public class Project
    {
        [Key]
        public int Project_id { get; set; }
        public required string Project_name { get; set; }
        public required string Project_status { get; set; }

        // Foreign Keys
        public int User_id { get; set; }
        public User User { get; set; }

        public int? Member_id { get; set; }
        public Member Member { get; set; }

        public int? Tag_id { get; set; }
        public Tag Tag { get; set; }
        public int? Task_id { get; set; }
        public Task Task { get; set; }
        public int? Announce_id { get; set; }
        public Announce Announce { get; set;
    }
}
