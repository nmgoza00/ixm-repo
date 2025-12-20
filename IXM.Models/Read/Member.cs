using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models
{

    public class MMEMBER
    {
        [Key]
        public int MEMBERID { get; set; }
        public string? MEMBERNUM { get; set; }
        public int? MEMBERTYPE { get; set; }
        public string? KNOWNAS { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string? IDTYPEID { get; set; }
        public string? IDNUMBER { get; set; }
        public string? CITYID { get; set; }
        public string? EMPNUMBER { get; set; }
        public string? EXTNUMBER { get; set; }
        public string? INITIALS { get; set; }
        public int? ETHNICID { get; set; }
        public int? COMPANYID { get; set; }
        public string? GENDER { get; set; }
        public string? DOB { get; set; }
        public int? MAGE { get; set; }
        public string? HTEL_NO { get; set; }
        public double? SALARY { get; set; }
        public int? YOS { get; set; }
        public string? CELLNUMBER { get; set; }
        public int? LOCALITYID { get; set; }
        public int? SECTORID { get; set; }
        public DateTime? EMPLOYDATE { get; set; }
        public DateTime? JOINDATE { get; set; }
        public double? ANNUAL_SALARY { get; set; }
        public int? POSITIONID { get; set; }
        public int? UNIONID { get; set; }
        public int? LANGUAGEID { get; set; }
        public int? QFCTID { get; set; }
        public string? TITLE { get; set; }
        public string? MARSTAT { get; set; }
        public string? EMAIL { get; set; }
        public string? MMTRID { get; set; }
        public int? BCOMPANYID { get; set; }
        public int? RUNSTATUSID { get; set; }   // Member Status ID
        public int? MEMSTATUSID { get; set; }   // Member Status ID
        public int? PROSTATUSID { get; set; }   // Process Status ID
        public int? LGLSTATUSID { get; set; }   // Legal Status ID
        public int? RECRUITERID { get; set; }   // Legal Status ID     
        public int? RMBLDID { get; set; }   // Remittance ID  
        public double? ICONTRIBUTION { get; set; }
        public double? DBCONTRIBUTION { get; set; }
        public string? FILENAME { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MemCardReceived { get; set; }

    }


    public class MMCDD
    {

        [Key]
        public int CDDID { get; set; }
        public int CDID { get; set; }
        public int MEMBERID { get; set; }
        public string PNAME { get; set; }
        public string PSURNAME { get; set; }
        public string WI { get; set; }
        public DateTime? INSDAT { get; set; }
        public string INSBY { get; set; }
        public int LT { get; set; }

    }


    public class MMEMBER_NOT
    {
        [Key]
        public int NOTEID { get; set; }
        public int MEMBERID { get; set; }
        public string? NOTES { get; set; }
        public string? NOTETYPE { get; set; }
        public string? CELLNUMBER { get; set; }
        public string? SOURCEOBJ { get; set; }
        public string? SOURCEID { get; set; }
        public string? STATUSID { get; set; }
        public string? PNOTEID { get; set; }
        public string? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }

    }

    public class MMEMBER_ADD
    {
        [Key]
        public int ADDID { get; set; }
        public int MEMBERID { get; set; }
        public string? ADDTYPE { get; set; }
        public string? STREETNO { get; set; }
        public string? STREETNOPF { get; set; }
        public string? STREETNAME { get; set; }
        public string? BUILDINGNO { get; set; }
        public string? BUILDINGNAME { get; set; }
        public string? POSTALCODE { get; set; }
        public string? CITYID { get; set; }
        public string? POBOXPREFIX { get; set; }
        public string? POBOXNO { get; set; }
        public string? POBOXNAME { get; set; }
        public string? POCITYID { get; set; }
        public string? POPOSTALCODE { get; set; }
        public string? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }

    }

    public class MMEMBER_BANK
    {
        [Key]
        public int MEMBANKID { get; set; }
        public int MEMBERID { get; set; }
        public int BANKNAMEID { get; set; }
        public required string ACCOUNTNUMBER { get; set; }
        public required string BRANCHCODE { get; set; }
        public string? DDOP { get; set; }
        public int DEDUCTIONSTATUSID { get; set; }
        public string? NOTES { get; set; }
        public string? INSERT_DATE { get; set; }
        public string? INSERTED_BY { get; set; }
        public string? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }

    }

    public class MEMBER_MASTERDATA
    {
        public List<MINTPOSITION> MINTPOSITION { get; set; }
        public List<VMSTATUS> VMSTATUS { get; set; }
        public List<MSECTOR> MSECTOR { get; set; }
        public List<MEMPLOYEE> MEMPLOYEE { get; set; }
}


}
