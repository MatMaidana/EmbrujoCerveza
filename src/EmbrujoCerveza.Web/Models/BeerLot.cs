using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmbrujoCerveza.Web.Models
{
    public class BeerLot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Estilo")]
        [Required]
        public int BeerStyleId { get; set; }

        public BeerStyle? BeerStyle { get; set; }

        [Display(Name = "Tipo de botella")]
        [Required]
        public int BottleTypeId { get; set; }

        public BottleType? BottleType { get; set; }

        [Display(Name = "Cantidad de botellas")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int BottleCount { get; set; }

        [Display(Name = "Fecha de envasado")]
        [DataType(DataType.Date)]
        public DateTime? BottledOn { get; set; }

        [Display(Name = "Notas")]
        [StringLength(300)]
        public string? Notes { get; set; }
    }
}
