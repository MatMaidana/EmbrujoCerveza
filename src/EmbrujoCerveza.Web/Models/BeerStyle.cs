using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EmbrujoCerveza.Web.Models
{
    public class BeerStyle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre del estilo")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "ABV (%)")]
        [Range(0, 100)]
        public decimal Abv { get; set; }

        [Display(Name = "IBU")]
        [Range(0, 1000)]
        public int? Ibu { get; set; }

        [Display(Name = "Imagen")]
        public string? ImageFileName { get; set; }

        public ICollection<BeerLot> Lots { get; set; } = new List<BeerLot>();

        [Display(Name = "Botellas registradas")]
        public int TotalLotBottles => Lots?.Sum(lot => lot.BottleCount) ?? 0;

    }
}
