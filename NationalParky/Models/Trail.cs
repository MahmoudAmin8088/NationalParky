using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParky.Models
{
    public class Trail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevaition { get; set; }

        public enum DifficultyType { Easy, Moderate, Difficult, Expert }
        public DifficultyType difficulty { get; set; }
        [Required]
        public int nationalParkId { get; set; }
        [ForeignKey("nationalParkId")]
        public NationalPark NationalPark { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
