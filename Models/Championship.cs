using System.ComponentModel.DataAnnotations;
using DriftNewsParser.Data.Enum;

namespace DriftNewsParser.Models
{
    public class Championship
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public ChampionshipCategory Name { get; set; }    }
}
