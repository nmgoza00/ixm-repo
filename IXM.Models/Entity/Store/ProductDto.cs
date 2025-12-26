using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models.Store
{

    public partial class mProductDto : ObservableObject
    {

        public int ProductId { get; set; }
        public string? Description { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? ProductImage { get; set; }
        public decimal ProductPrice { get; set; }

        [ObservableProperty]
        public int _ProductQuantity;
        public DateTime? InsertDate { get; set; }
        public string? InsertedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public string? IsActive { get; set; }
    }
}
