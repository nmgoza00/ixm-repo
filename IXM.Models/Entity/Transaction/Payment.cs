using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models
{

    public static class TPAYMENTExtension
    {
        public static IQueryable<TPAYMENT> PaymentConfirm(this IQueryable<TPAYMENT> source)
        {
            return source.Select(b => new TPAYMENT
            {
                PAYMENTID = b.PAYMENTID,
                PAYMENTNUM = b.PAYMENTNUM,
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


        public static IQueryable<TPAYMENT_READ> ForCompanyPaymentRead(this IQueryable<TPAYMENT_READ> source)
        {
            return source.Select(b => new TPAYMENT_READ
            {
                CUSTOMERID = b.CUSTOMERID,
                COMPANYNUM = b.COMPANYNUM,
                DEBTORNAME = b.DEBTORNAME,
                PAYMENTID = b.PAYMENTID,
                PAYMENTNUM = b.PAYMENTNUM,
                PYEARMONTH = b.PYEARMONTH,
                IAMOUNT = b.IAMOUNT,
                BUILDINGNAME = b.BUILDINGNAME,
                POSTALCODE = b.POSTALCODE,
            });
        }
    }
    public static class TTRANWrite
    {
        public static IQueryable<TTRAN> PaymentConfirm(this IQueryable<TTRAN> source)
        {
            return source.Select(b => new TTRAN
            {
                TJRNLID = b.TJRNLID,
                TRANID = b.TRANID,
                PERIODID = b.PERIODID,
                TAMOUNT = b.TAMOUNT,
                QUANTITY = b.QUANTITY,
                LINENUM = b.LINENUM,
                CUSTOMERID = b.CUSTOMERID,
                FACCNO = b.FACCNO,
                MEMBERID = b.MEMBERID,
                INSERT_DATE = b.INSERT_DATE,
                INSERTED_BY = b.INSERTED_BY

            });
        }
    }
    public static class TTJRNWrite
    {
        public static IQueryable<TTJRN> PaymentConfirm(this IQueryable<TTJRN> source)
        {
            return source.Select(b => new TTJRN
            {
                TJRNLID = b.TJRNLID,
                TJRNLNUM = b.TJRNLNUM,
                TJRNLTYP = b.TJRNLTYP,
                PERIODID = b.PERIODID,
                DESCRIPTION = b.DESCRIPTION,
                INSBY = b.INSBY,
                INSDT = b.INSDT,
                HGUID = b.HGUID,

            });
        }
    }


    public class TPAYMENT_READ
    {
        [Key]
        public int PAYMENTID { get; set; }
        public required string PAYMENTNUM { get; set; }
        public int PERIODID { get; set; }
        public int CUSTOMERID { get; set; }
        public string? CUSTOMER_CODE { get; set; }
        public DateTime? PAYMENTDATE { get; set; }
        public DateTime? TRANDATE { get; set; }
        public string? DEBTORNAME { get; set; }
        public string? DEBTORTYPE { get; set; }
        public int? PSTATUSID { get; set; }
        public double? IAMOUNT { get; set; }
        public double? IADMINFEE { get; set; }
        public int? BUILDINGNO { get; set; }
        public string? BUILDINGNAME { get; set; }
        public string? POSTALCODE { get; set; }
        public string? PYEARMONTH { get; set; }
        public string? POBOXPREFIX { get; set; }
        public int? POBOXNO { get; set; }
        public string? INVOICENUM { get; set; }
        public string? PAYMENTREF { get; set; }
        public int? DBFACCTID { get; set; }
        public int? CRFACCTID { get; set; }
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
    public class TPAYMENT
    {
        [Key]
        public int PAYMENTID { get; set; }
        public required string PAYMENTNUM { get; set; }
        public int PERIODID { get; set; }
        public int CUSTOMERID { get; set; }
        public string? CUSTOMER_CODE { get; set; }
        public DateTime? PAYMENTDATE { get; set; }
        public DateTime? TRANDATE { get; set; }
        public string? DEBTORNAME { get; set; }
        public string? DEBTORTYPE { get; set; }
        public int? PSTATUSID { get; set; }
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
        public string? INVOICENUM { get; set; }
        public string? PAYMENTREF { get; set; }
        public int? DBFACCTID { get; set; }
        public int? CRFACCTID { get; set; }
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
    public class TPAYMENT_DET
    {
        [Key]
        public int PAYMENTDID { get; set; }
        public int PAYMENTID { get; set; }
        public int PERIODID { get; set; }
        public int CUSTOMERID { get; set; }
        public int MEMBERID { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? RMBLDID { get; set; }
        public string? CITYID { get; set; }
        public string? PROVINCEID { get; set; }
        public int? LOCALITYID { get; set; }
        public int? MEMSTATUSID { get; set; }
        public int? BCOMPANYID { get; set; }
        public double? SALARY { get; set; }
        public double? ERAMOUNT { get; set; }
        public double? IAMOUNT { get; set; }
        public DateTime? INSDT { get; set; }
        public string? INSBY { get; set; }
        public double? LTAMOUNT { get; set; }
        public int? LT { get; set; }
        public int? LTCNT { get; set; }

    }

    public class TTJRN
    {

        [Key]
        public int TJRNLID { get; set; }
        public string TJRNLNUM { get; set; }
        public string TJRNLTYP { get; set; }
        public string DESCRIPTION { get; set; }
        public string REFERENCEKEY { get; set; }
        public int PERIODID { get; set; }
        public int? TJSTATUSID { get; set; }
        public int? PTJRNLID { get; set; }
        public DateTime? INSDT { get; set; }
        public string INSBY { get; set; }
        public string HGUID { get; set; }

    }
    public class TTRAN
    {

        [Key]
        public int TRANID { get; set; }
        public int TJRNLID { get; set; }
        public int LINENUM { get; set; }
        public string? TRANTYPE { get; set; }
        public int PRODUCTID { get; set; }
        public int PERIODID { get; set; }
        public int CUSTOMERID { get; set; }
        public int MEMBERID { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? FACCNO { get; set; }
        public double? QUANTITY { get; set; }
        public double? TAMOUNT { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }

    }


}
