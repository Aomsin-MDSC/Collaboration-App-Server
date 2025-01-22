using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Member
    {
        [Key]
        public int Member_id { get; set; }
        public int? User_id { get; set; }
        public User? User { get; set; }

        public int Project_id { get; set; }
        public Project? Project { get; set; }
        public int Member_role { get; set; }

    }
}
