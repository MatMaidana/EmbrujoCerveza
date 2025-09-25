using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmbrujoCerveza.Web.Models
{
    public class BottleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name = "Material")]
        public string Material { get; set; } = string.Empty;

        [Display(Name = "Capacidad (ml)")]
        [Range(50, 5000)]
        public int CapacityMl { get; set; }

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        public ICollection<BeerLot> Lots { get; set; } = new List<BeerLot>();

        [Display(Name = "Etiqueta")]
        public string DisplayName => $"{CapacityMl} ml - {Material}";
    }
}
