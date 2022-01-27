using System.ComponentModel.DataAnnotations;

namespace APIBookstore.Api.Dtos
{
    public class ProductDTO
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Nome")]
        [StringLength(80, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 2)]
        [Required(ErrorMessage = "O campo {0} é obrigatorio.")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Preço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Price { get; set; }


        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1,500)]
        public int Quantity { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(80, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 2)]
        public string Category { get; set; }


        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Img { get; set; }

    }
}
