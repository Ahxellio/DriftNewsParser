using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriftNewsParser.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }

        [ForeignKey(nameof(Championship))]
        public int ChampionshipId { get; set; }
        [Required]
        public Championship Championship { get; set; }
    }
}
