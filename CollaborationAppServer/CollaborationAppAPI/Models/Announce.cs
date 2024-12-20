using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Announce
    {
        [Key]
        public int Announce_id { get; set; }
        public required string Announce_text { get; set; }
        public required DateTime Announce_date { get; set; }
        public int? Project_id { get; set; }
        public Project? Project { get; set; }
    }
}
