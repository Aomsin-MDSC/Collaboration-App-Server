using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Tag
    {
        [Key]
        public int Tag_id { get; set; }
        public required string Tag_name { get; set; }
        public required string Tag_color { get; set; }
        public ICollection<Project>? Projects { get; set; }
        public Task? Task { get; set; }

    }
}
