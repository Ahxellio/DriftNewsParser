using DriftNewsParser.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriftNewsParser.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CarName { get; set; }
        [Required]
        public string CarEngine { get; set; }
        [Required]
        public string Number { get; set; }

        public string Team { get; set; }
        public string Href { get; set; }

        [Required]
        public string ImgSrc { get; set; }

        [ForeignKey(nameof(Championship))]
        public int ChampionshipId { get; set; }
        [Required]
        public ChampionshipCategory Championship { get; set; }

    }
}
