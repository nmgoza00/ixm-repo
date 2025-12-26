using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IXM.Models.Store;


public partial class tCartItems : ObservableObject
{
    [Key]
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string? Description { get; set; }
    public int? ProductCategoryId { get; set; }
    public string? ProductImage { get; set; }
    public decimal ProductPrice { get; set; }
    public DateTime? InsertDate { get; set; }
    public string? InsertedBy { get; set; }

    [ObservableProperty, NotifyPropertyChangedFor(nameof(Amount))]
    private int _productQuantity;

    public decimal Amount => ProductPrice * ProductQuantity;
}
