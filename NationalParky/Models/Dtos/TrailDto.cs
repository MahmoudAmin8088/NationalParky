using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static NationalParky.Models.Trail;

namespace NationalParky.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevaition { get; set; }

        public DifficultyType difficulty { get; set; }
        [Required]
        public int nationalParkId { get; set; }
        public NationalParkDto NationalPark { get; set; }
    }
}
