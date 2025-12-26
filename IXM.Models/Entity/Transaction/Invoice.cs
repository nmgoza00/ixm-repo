using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IXM.Models
{

    public static class TINVOICEExtension
    {
        public static IQueryable<TINVOICE> InvoiceConfirm(this IQueryable<TINVOICE> source)
        {
            return source.Select(b => new TINVOICE
            {
                INVOICEID = b.INVOICEID,
                INVOICENUM = b.INVOICENUM,
                CUSTOMERID = b.CUSTOMERID,
                DEBTORNAME = b.DEBTORNAME,
                IAMOUNT = b.IAMOUNT,               
                INSERTED_BY = b.INSERTED_BY,
                MODIFIED_BY = b.MODIFIED_BY,
                INSERT_DATE = b.INSERT_DATE,
                MODIFIED_DATE = b.MODIFIED_DATE,
                HGUID = b.HGUID,

            });
        }
    }

    public class TINVOICE
        {
            [Key]
            public int INVOICEID { get; set; }
            public required string INVOICENUM { get; set; }
            public int PERIODID { get; set; }
            public int CUSTOMERID { get; set; }
            public string? CUSTOMER_CODE { get; set; }
            public DateTime? PAYMENTDATE { get; set; }
            public DateTime? TRANDATE { get; set; }
            public string? DEBTORNAME { get; set; }
            public string? DEBTORTYPE { get; set; }
            public int? ISTATUSID { get; set; }
            public double? IAMOUNT { get; set; }
            public double? IADMINFEE { get; set; }
            public int? SOURCEID { get; set; }
            public string? SOURCE_TRANTYPE { get; set; }
            public int? STREETNO { get; set; }
            public string? STREETNAME { get; set; }
            public int? BUILDINGNO { get; set; }
            public string? BUILDINGNAME { get; set; }
            public string? POSTALCODE { get; set; }
            public string? POBOXNAME { get; set; }
            public string? POBOXPREFIX { get; set; }
            public int? POBOXNO { get; set; }
            public string? USE_PO { get; set; }
            public string? USE_PA { get; set; }
            public string? NOTES { get; set; }
            public int? DBFACCTID { get; set; }
            public int? CRFACCTID { get; set; }
            public int? PSTGL { get; set; }
            public int? PAYMENTID { get; set; }
            public int? TJRNLID { get; set; }
            public string? HGUID { get; set; }
            public DateTime? INSERT_DATE { get; set; }
            public string? INSERTED_BY { get; set; }
            public DateTime? MODIFIED_DATE { get; set; }
            public string? MODIFIED_BY { get; set; }
        //Mainly for Mobile App..linvestigate
        [NotMapped]
            public string? CNAME { get; set; }
            [NotMapped]
            public string? COMPANYNUM { get; set; }

    }
        public class TINVOICE_DET
        {
            [Key]
            public int INVOICEDID { get; set; }
            public int INVOICEID { get; set; }
            public int PERIODID { get; set; }
            public int CUSTOMERID { get; set; }
            public int PRODUCTID { get; set; }
            public int MEMBERID { get; set; }
            public string? DESCRIPTION { get; set; }
            public int? PAYMENTDID { get; set; }
            public string? CITYID { get; set; }
            public string? PROVINCEID { get; set; }
            public int? LOCALITYID { get; set; }
            public int? MEMSTATUSID { get; set; }
            public int? BCOMPANYID { get; set; }
            public int? INVINDEX { get; set; }
            public double? SELLPRICE { get; set; }
            public double? IAMOUNT { get; set; }
            public DateTime? INSDT { get; set; }
            public string? INSBY { get; set; }
            public DateTime? MODAT { get; set; }
            public string? MODBY { get; set; }
            public int? LT { get; set; }

    }

}
