using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Member
    {
        [Key]
        public int Member_id { get; set; }
        public required string Member_role { get; set; }
        public int? Project_id { get; set; }
        public Project Project { get; set; }

    }
}
