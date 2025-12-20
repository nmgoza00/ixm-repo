using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using IXM.Constants;

namespace IXM.Models
{

    public class CONNECT_ENVIROMENT
    {
        [Key]
        public int CEID { get; set; }
        public string DESCRIPTION { get; set; }
        public string SERVER { get; set; }
        public string PORT { get; set; }
    }

    public class MUSER_GROUP
    {
        [Key]
        public int UGID { get; set; }
        public required string DESCRIPTION { get; set; }
        public required string UGCODE { get; set; }
    }


    public class CUSER_GROUP
    {
        [Key]
        public int UGID { get; set; }
        public int USERID { get; set; }
        public string?   AUTHCODE { get; set; }
        public required string DESCRIPTION { get; set; }
        public required string UGCODE { get; set; }
    }


    public class CUSER_GROUP_WRITE
    {
        [Key]
        public int UGID { get; set; }
        public int USERID { get; set; }
    }


    public class ApplicationUser : IdentityUser
    {
        public int? SYSTEM_USERID { get; set; }
        public string? SYSTEM_UNAME { get; set; }
        public string? REGISTRATIONTYPE { get; set; }

    }

    public class CstmRoles : IdentityRole
    {
        public required string? ROLE_CODE { get; set; }

    }



    public class CstmUserRole
    {
        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string? UserName { get; set; }

        public string? RoleName { get; set; }

    }




    public class ID_Users
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CellNumber { get; set; }
        public string RoleName { get; set; }
        public string UserStatus { get; set; }
        public string? SystemName { get; set; }
        public string Employee_FullName { get; set; }
        public int EmployeeId { get; set; }
        public int? System_UserId { get; set; }
        public string? System_UNAME { get; set; }
        public string InsertDate { get; set; }
        public string Logins { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }


    public class ID_Roles
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }

    public class ID_UserRoles
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string? UGCode { get; set; }
    }

    public class ID_UserSystems
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string UserName { get; set; }
    }


    public class MUSER_DEVICE
    {
        [Key]
        public string UNAME { get; set; }
        public string EMAIL { get; set; }
        public string? DEVICETYPE { get; set; }
        public string? MODEL { get; set; }
        public string? NAME { get; set; }
        public string? MANUFACTURER { get; set; }
        public string? OSVERSION { get; set; }
        public string? IDIOM { get; set; }
        public string? PLATFORM { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public int? LOGINS { get; set; }
        public string? INFOGUID { get; set; }

    }

    public class MUSER_MENU
    {
        [Key]
        public string MENUID
        {
            get; set;
        }
        public string? PARENTID
        {
            get; set;
        }
        public int USERID { get; set; }

        public string SYSTEMUSERNAME { get; set; }
        public int? LEVEL_L { get; set; }
        public string? FRIENDLYNAME { get; set; }
        public string? FORMHEADER { get; set; }
        public string? TECHNICALNAME { get; set; }
        public string? FOBJECT { get; set; }
        public string? FTYPE { get; set; }
        public string? AR_ADD { get; set; }
        public string? AR_EDIT { get; set; }
        public string? AR_DELETE { get; set; }
        public string? IMAGEURL { get; set; }
        public int? MSTATUSID { get; set; }
        public string? ISACTIVE { get; set; }
        public DateTime? EXPDAT { get; set; }
    }

    public class REGISTER_DATA
    {
        public string? userName { get; set; }
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
        public string? name { get; set; }
        public string? surname { get; set; }
        public string? password { get; set; }
        public string? confirmpassword { get; set; }
        public string? client { get; set; }

        public string? BU_Company { get; set; }
        public string? BU_ContactPerson { get; set; }
        public string? BU_RegistrationCode { get; set; }
        public string? NU_Union { get; set; }
        public int? NU_System { get; set; }
        public IxmRegisterUserType RegistrationType { get; set; }

        public DEVICE_INFO? deviceInfo { get; set; }

    }


    public class USER_PASSWORDRESET
    {
        public string token { get; set; }
        public string email { get; set; }
        public string message { get; set; }
        public string? password { get; set; } = string.Empty;
        public string? confirmpassword { get; set; } = string.Empty;

    }


    public class UserPasswordResets
    {
        [Key]
        [Column(Order = 1)]
        public required string UserId { get; set; }
        public required string? LoginProvider { get; set; }
        [Key]
        [Column(Order = 2)]
        public required string PasswordResetToken { get; set; }
        public required DateTime PasswordResetExpiryDate { get; set; }
        public DateTime? PasswordResetVerifiedDate { get; set; }
        public required string? PasswordSalt { get; set; }

    }


    public class UserDetails
    {
        public static string UserGuid { get; set; } = "";
        public static string CurrentUser { get; set; } = "";
        public static string UserEmail { get; set; } = "";
        public static string UserRoles { get; set; } = "";
        public static int SystemUserId { get; set; } = -99;
        public static string? CurrentEmail { get; set; } = "";
        public static string SystemId { get; set; } = "";
        public static string CompanyId { get; set; } = "";
        public static IEnumerable<int> CompanyIdList { get; set; }
        public static List<USER_COMPANY> CompanyInfoList { get; set; }

    }


    public class USER_MASTERDATA
    {
        public List<MINTPOSITION> MINTPOSITION { get; set; }
        public List<VMSTATUS> VMSTATUS { get; set; }
        public List<MLOCALITY> MLOCALITY { get; set; }
        public List<MUSER_GROUP> MUSER_GROUP { get; set; }
        public List<MEMPLOYEE> MEMPLOYEE { get; set; }
    }


    public class MSYSTEM
    {

        [Key]
        public string SYSTEMID { get; set; }
        public string SYSTEMNAME { get; set; }
    }


    public class MPROCESSOR
    {
        public string USERID { get; set; }
        public string UNAME { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string FULLNAME
        {
            get { return NAME.ToString() + ", " + SURNAME.ToString(); }
        }
    }


    public class MEMPLOYEE
    {
        [Key]
        public int EMPID { get; set; }
        public int? EMPTYPID { get; set; }
        public string? MNAME { get; set; }
        public string? MSURNAME { get; set; }
        public string FULLNAME
        {
            get { var sVal1 = MNAME != null ? MNAME.ToString() : "";
                  var sVal2 = MSURNAME != null ? MSURNAME.ToString() : "";
                return sVal1 + ""+ sVal2;
            }
        }
        public string? IDNUMBER { get; set; }
        public string? CELLNUMBER { get; set; }
        public string? EMAIL { get; set; }
        public int? IPOSITIONID { get; set; }
        public int? LOCALITYID { get; set; }
        public string? MEMBERID { get; set; }
        public int? USERID { get; set; }

    }


    public class CstmUserList
    {
        public required string? USERSTATUS { get; set; }
        public required string? SYSTEMNAME { get; set; }
        public required string? ROLENAME { get; set; }
        public required string? UserId { get; set; }
        public required string? USERNAME { get; set; }
        public required string? EMAIL { get; set; }
        public required string? PHONENUMBER { get; set; }

    }

    public class MUSER_ROLE
    {

        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string RoleId { get; set; }
    }

    public class MUSER_SYSTEM
    {
        [Key]
        public string UserId { get; set; }
        public string SystemId { get; set; }
        public int? MUSER_USERID { get; set; }
        public string? INSERTED_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string? ISDOMAINUSER { get; set; }

    }

    public class RegistrationPayload
    {
        public string CompanyGUId { get; set; }
        public int SystemId { get; set; }
        public DateTime IssuedAt { get; set; }
    }

}
