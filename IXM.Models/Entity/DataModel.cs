using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models
{
    public class MasterData
    {
    }

    public class MUSER
    {

        [Key]
        public int USERID { get; set; }
        public string UNAME { get; set; }
        public string? UPASSWORD { get; set; }
        public string? NAME { get; set; }
        public string? SURNAME { get; set; }
        public string? TELNUMBER { get; set; }
        public string? CELLNUMBER { get; set; }
        public int? LOCALITYID { get; set; }
        public int? IPOSITIONID { get; set; }
        public int? UGID { get; set; }
        public int? MEMBERID { get; set; }
        public int? EMPID { get; set; }
        public int? MSTATUSID { get; set; }
        [EmailAddress]
        public string? EMAILADDRESS { get; set; }
        public DateOnly? EXPDAT { get; set; }
        public string? AUTHCODE { get; set; }
        public string? APPSERVICE { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MCNAME { get; set; }
        public string? ISACTIVE { get; set; }
        [NotMapped]
        public string UGCODE { get; set; }
        [NotMapped]
        public string? SYSCODE { get; set; }
        [NotMapped]
        public string OBJECTID { get; set; }
        [NotMapped]
        public string DOCUMENTNAME { get; set; }
        [NotMapped]
        public string SFOLDERNAME { get; set; }
        [NotMapped]
        public string IMAGEURI { get; set; }

    }

    public class MUSERLst
    {
        public List<MUSER> MUSERs { get; set; }
    }

    public class STATSREGION
    {
        [Required]
        public int MEMBERS { get; set; }
        public int LOCALITYID { get; set; }
        public string REGIONAME { get; set; }
        public string MIN_FYEARMONTH { get; set; }
        public string MAX_FYEARMONTH { get; set; }

    }


    public class STATSBRANCH_ITEM
    {
        [Key]
        public int PERIODID { get; set; }
        public string? MYEARMONTH { get; set; }
        public int? USERID { get; set; }
        public int? MMEMCOUNT { get; set; }
        public string? PAYMENTNUM { get; set; }
        public double? IAMOUNT { get; set; }
        public double? PMEMCOUNT { get; set; }

    }
    public class STATSBRANCH
    {
        [Key]
        public int PERIODID { get; set; }
        public int COMPANYID { get; set; }
        public string? MYEARMONTH { get; set; }
        public string? COMPANYNUM { get; set; }
        public string? CNAME { get; set; }
        public int? USERID { get; set; }
        public int? MMEMCOUNT { get; set; }
        public string? PAYMENTNUM { get; set; }
        public double? IAMOUNT { get; set; }
        public double? PMEMCOUNT { get; set; }

    }

    public class STATSBRANCH_HEADER
    {
        [Key]
        public int COMPANYID { get; set; }
        public string COMPANYNUM { get; set; }
        public string CNAME { get; set; }
        public List<STATSBRANCH_ITEM> Items = new();
    }


    public class REP_PROCESSORWORK
    {

        [Key]
        public int ROW_NUMBER { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string? PAYMENT { get; set; }
        public string PERIODID { get; set; }
        public string USER_COMPANY { get; set; }
        public string CNAME { get; set; }
        public string? MYEARMONTH { get; set; }
        public string? USERID { get; set; }
        public string? PAYMENTID { get; set; }
        public string? ISLOADED { get; set; }

    }



    public class IMAGEUPLOAD
    {
        public string ByteBase64 { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

    }

    public class UserConnectInfo
    {
            public int UserId { get; set; }    
            public string Username { get; set; }
            public string UPassword { get; set; }
            public string Fullname { get; set; }        
            public string Email { get; set; }
            public string LocalityId { get; set; }
            public int RoleID { get; set; }
            public string RoleText { get; set; }

    }

    public class IxmAppInfo
    {  
                public string BaseAPIURL { get; set; }
                public string BaseURL { get; set; }
    }

    public class ORGANISER_MENU
    {
        public string MENUCODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string IMAGE { get; set; }
        public string PAGENAME { get; set; }
        public bool ENABLED { get; set; }
    }

    public class TDSURVEY_C
    {
        [Key]
        public int SURVEYDID { get; set; }
        public int SURVEYID { get; set; }
        public string? FIELDNAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? DATATYPE { get; set; }
        public string? FIELDVALUE { get; set; }
        public DateTime? INSERTDATE { get; set; }
    }


    public class TSURVEYMEMBERDATA
    {
        public int MEMBERID { get; set; }
        public int SURVEYID { get; set; }
        public int? BCOMPANYID { get; set; }
        public DateTime? SURVEYDATE { get; set; }
        public string? SRVY_QU01 { get; set; }
        public string? SRVY_QU02 { get; set; }
        public string? SRVY_QU03 { get; set; }
        public string? SRVY_QU04 { get; set; }
        public string? SRVY_QU05 { get; set; }
        public string? SRVY_QU06 { get; set; }
        public string? SRVY_QU07 { get; set; }
        public string? SRVY_QU08 { get; set; }
        public string? SRVY_QU09 { get; set; }
        public string? SRVY_QU10 { get; set; }
    }


}

