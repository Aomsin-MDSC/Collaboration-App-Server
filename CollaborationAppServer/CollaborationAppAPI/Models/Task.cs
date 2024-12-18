using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Task
    {
        [Key]
        public int Task_id { get; set; }
        public required string Task_name { get; set; }
        public required string Task_detail { get; set; }
        public required string Task_end { get; set; }
        public required string Task_owner { get; set; }
        public required string Task_color { get; set; }
        public required string Task_status{ get; set; }
        public int? Comment_id { get; set; }
        public Comment Comment { get; set; }
        public int? Tag_id { get; set; }
        public Tag Tag { get; set; }
        public int? Announce_id { get; set; }
        public Announce Announce { get; set; }

    }
}
