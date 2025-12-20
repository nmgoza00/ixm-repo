
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace IXM.Models.Store
{



    public class mProduct
    {
        [Key]
        public int ProductId { get; set; }
        public string? Description { get; set; }
        public int? ProductCategoryId { get; set; }
        [NotMapped]
        public string? ProductImage { get; set; }
        public DateTime? InsertDate { get; set; }
        public string? InsertedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? IsActive { get; set; }
    }


    public class mProductDisplay
    {
        [Key]
        public int ProductId { get; set; }
        public string? Description { get; set; }
        public int? ProductCategoryId { get; set; }
        [NotMapped]
        public string? ProductImage { get; set; }
        public decimal? ProductPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public string? InsertedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? IsActive { get; set; }
    }


    public class mProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public Blob? CategoryImage { get; set; }
        public DateTime InsertDate { get; set; }
        public string? InsertedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? IsActive { get; set; }
    }

    public class cSystemProduct
    {
        [Key]
        public int SystemId { get; set; }
        public int ProductId { get; set; }
        public DateTime InsertDate { get; set; }
        public string? InsertedBy { get; set; }
    }



}
