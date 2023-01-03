using System.ComponentModel.DataAnnotations;

namespace MODULOAPI.Entities
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Telephone { get; set; }
        public bool Active { get; set; }
    }
}