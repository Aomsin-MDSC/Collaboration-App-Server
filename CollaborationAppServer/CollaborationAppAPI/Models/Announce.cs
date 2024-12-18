﻿using System.ComponentModel.DataAnnotations;

namespace CollaborationAppAPI.Models
{
    public class Announce
    {
        [Key]
        public int Announce_id { get; set; }
        public required string Announce_text { get; set; }
    }
}