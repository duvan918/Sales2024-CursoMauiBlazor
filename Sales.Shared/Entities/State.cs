using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Estado/Departamento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres.")]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }

        public Country? Country { get; set; }   // Muchos estados pertenecen a 1 país -> N-1

        public ICollection<City>? Cities { get; set; }     //Un estado tiene muchas ciudades -> 1-N

        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}
