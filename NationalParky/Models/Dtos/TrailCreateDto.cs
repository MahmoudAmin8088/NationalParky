using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static NationalParky.Models.Trail;

namespace NationalParky.Models.Dtos
{
    public class TrailCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevaition { get; set; }

        public DifficultyType difficulty { get; set; }
        [Required]
        public int nationalParkId { get; set; }
    }
}
