﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Villa_API.Models.DTO
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set;}
    }
}
