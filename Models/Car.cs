using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriftNewsParser.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string CarBrand { get; set; }
        public string Engine { get; set; }
    }
}
