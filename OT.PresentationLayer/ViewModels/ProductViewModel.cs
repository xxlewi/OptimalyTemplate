using System.ComponentModel.DataAnnotations;

namespace OT.PresentationLayer.ViewModels;

public class ProductViewModel : BaseViewModel
{
    [Required(ErrorMessage = "Název produktu je povinný")]
    [StringLength(100, ErrorMessage = "Název může mít maximálně 100 znaků")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Popis může mít maximálně 500 znaků")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cena je povinná")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena musí být větší než 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Skladové množství je povinné")]
    [Range(0, int.MaxValue, ErrorMessage = "Skladové množství nemůže být záporné")]
    public int Stock { get; set; }
}