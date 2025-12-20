
namespace IXM.Constants
{


    public enum IXMMailType
    {
        Register,
        AccountActivated,
        ForgotPassword,
        PasswordChanged,
        Resend,
        RemmittanceSubmitted,
        RemmittanceReceived,
        CompanyRegistration,
    }

    public enum IxmRegisterUserType
    {
        BusinessUser,
        NormalUser,
        EmployeeUser
    }
    public enum IxmDownLoadType
    {
        Object,
        Filename
    }
    public enum IxmDataCRUDType
    {
        Insert,
        Update,
        Delete,
        Upsert
    }

    public enum IxmScriptExecutionType
    {
        RemittanceUploadUpdate,
        LastPeriodUpdate,
        StatusUpdate,
        GenderUpdate,
        AgeUpdate
    }
    public enum IxmUploadType
    {
        UploadRemittance,
        UploadEventPage,
        UploadProofOfPayment,
        UploadMailCorrespondent,
        UploadDigitalSignature,
        UploadUserImage,

    }

    public enum IxmBlockScriptType
    {
        BS_UpdatePostPayment,
        BS_ReadCompanyDetails,
        BS_UpdateTISSU
    }

    public enum IxmAppLogType
    {
        LogRemittance,
        LogInvoicing,
        LogPayment,
        LogEvents,
        LogB2B,
        LogCredits,
        LogSystem
    }
    public enum IxmAppSourceObjects
    {
        B2BLOADS,
        TRMBL,
        MCOMPANY,
        TPAYMENT,
        MMEMBER,
        TEVENT
    }
    public enum IxmAppDocumentType
    {
        XLS_TEMPLATE,
        XLS_REPORT,
        OTH_REMITTANCE,
        XLS_REMITTANCE,
        EVENT,
        PROJECT,
        XLS_INVOICE,
        PDF_STATEMENT,
        XLS_REMITTANCEERR,
        MEMPHOTO,
        MEMTERM,
        OTHR1,
        PAYXLS,
        PAYMSG,
        REMITTANCE_POP,
        REMITTANCE_EMAIL,
        USERPHOTO,
        DIGISIGNATURE,
        MEMAPPLICATION,
        MEMPAYSLIP,
        MEMTERMINATION,
        EMPLOYEEPHOTO,
        EVENTCARD
    }

    /*
     * 
    public readonly record struct DailyTemperature(IxmAppDocumentType docType, string SOURCEOBJ, string SOURCEFLD);
     * public static DailyTemperature[] data = [
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID"),
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID"),
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID"),
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID"),
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID"),
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID"),
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID"),
    new DailyTemperature(IxmAppDocumentType.DIGISIGNATURE,"MUSER","USERID")
    ];*/

    public enum IxmUserRights
    {
        MB_B_B2B,
        MB_G_SEC,
        MB_G_MEM,
        MB_G_ORG,
        MB_G_PRO,
        MB_G_GEN,
        G_G_SUPER_USER,
        L_MUR_NATIONAL_O,
        P_RGR_PROVINCIAL,
        L_DGR_DATA_USER,
        L_DGR_MATERIAL_R,
        N_RGR_REPORTS_US
    }

    public enum IxmAppTemplateFileType
    {
        Schedule,
        Invoice,
        ApplicationForm,
        ClaimForm,
        ResignationForm,
        Report,
        None
    }

    public enum IxmAppCaptionObject
    {
        TRMBLE,
        MEMPOYEE,
        MMTR,
        MCOMPANY,
        BANK_STATEMENT,
        SCHEDULEBLOCK,
        MLOCALITY,
        TPRJEVTD,
        BCOMPANY,
        SCHEDULECV,
        MPERIOD,
        MSTATUS,
        TEMPLATE,
        CGROUP_RIGHTS,
        MUSER_GROUP,
        MUSER,
        SCHEDULE,
        TRMBLD,
        TRMBL,
        MMEMBER,
        MONTHLYREPORT
    }


    public enum IxmDBSequence
    {
        SEQTRMBL,
        SEQTRMBLD,
        SEQTRMBLE,
        SEQOBJECT_STA,
        SEQMOBJECT_DOC,
        SEQ_MUSER,
        SEQTPAYMENT,
        SEQMEMBER,
        SEQEMPLOYEE,
        SEQ_SSMSDATA,
        SEQ_TMCDD,
        SEQTTJRN,
        SEQTPRJEVT,
        SEQTPROJECT,
        SEQTEVENT,
        SEQPROCESSRUN,
        SEQMSECTOR,
        SEQMUNION,
        SEQMLOCALITY,
        SEQMCOMPANYTPE,
        SEQMINTPOSITION,
        SEQMCASETYPE,
        SEQMPERIOD,
        SEQTRANID
    }
    public enum IxmRemittanceMatchTypes
    {
        MATCH_BCOMPANYID,
        MATCH_COMPANYID,
        MATCH_EMPNUMBER,
        MATCH_IDNUMBER,
        MATCH_GENDER,
        MATCH_NAME,
        MATCH_SURNAME,
        MATCH_CITYID,
        MATCH_MEMSTATUSID
    }


    public enum IxmStaticReports
    {
        REP_FINCOM,
        REP_APPFORM
    }

    public enum IxmTaskProcess
    {
        TASK_STARTED,
        TASK_FAILED,
        TASK_COMPLETED
    }

    public enum IxmPageComponents
    {
        CmptRemittanceList
    }

    public class IxmReturnStatus
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }

    public class ConstantFiles
    {
        public static readonly string ScheduleTemplate = "ScheduleTemplate.xlsx";
    }

    public class CommonConstants
    {
        public class FileProperties
        {
            public static int MaxFileSize =  1024 * 1024 * 3;
            public static int MaxAllowedFiles = 3;
        }

        public static string RegistrationCodeKey { get; set; } = "MySuperSecretKey123!";
        public static string? JWToken { get; set; } = "";
        public static string? CurrentUser { get; set; } = "";
        public static string? ConnectedEnvironment { get; set; } = "";
        public static int? EnviromentID { get; set; } = null;
        public static string? EnviromentPort { get; set; } = null;
        //public static string? BaseAddress { get; set; } = "https://ilungu.co.za:" + EnviromentPort + "/";
        //public static string? BaseAddress { get; set; } = "https://ixm.sytes.net:6080:" + EnviromentPort + "/";
        public static string? BaseAddress { get; set; } = "https://ixm.sytes.net:6080/";
        public static int ScheduleRowStart { get; set; } = 0;

        public static string BaseAPIUrl { get => $"{BaseAddress}api/";}

        //public static readonly string BaseAddress =
        //public static readonly string BaseAPIUrl = $"{BaseAddress}api/";
        public static string UserAuthURL { get => $"{BaseAPIUrl}User/UserAuth"; }
        public static string OrganizerCompanyURL { get => $"{BaseAPIUrl}Organizer/Companies"; }
        public static string OrganizerBranchURL { get => $"{BaseAPIUrl}Organizer/Branches/Guid"; }
        public static string OrganizerUnmappedCompaniesURL { get => $"{BaseAPIUrl}Organizer/Companies/Unmapped"; }
        public static string OrganizerUnmappedBranchesURL { get => $"{BaseAPIUrl}Organizer/Branches/Unmapped"; }
        public static string CompanyList { get => $"{BaseAPIUrl}Company"; }
        public static string CompanyMembers { get => $"{BaseAPIUrl}Company/Members"; }
        public static string CompanyBySystem { get => $"{BaseAPIUrl}Company/System"; }
        public static string Member { get => $"{BaseAPIUrl}Member"; }
        public static string MemberGuid { get => $"{BaseAPIUrl}Member/Guid"; }
        public static string MemberDetails { get => $"{BaseAPIUrl}Member/Details"; }
        public static string MemberGlobalFind { get => $"{BaseAPIUrl}Member/GlobalFind"; }
        public static string MemberByCompany { get => $"{BaseAPIUrl}Member/Company"; }
        public static string BranchList { get => $"{BaseAPIUrl}Branch"; }
        public static string SectorList { get => $"{BaseAPIUrl}MasterData/Sector"; }
        public static string PeriodList { get => $"{BaseAPIUrl}MasterData/Period"; }
        public static string TableCaption { get => $"{BaseAPIUrl}MasterData/Caption"; }
        public static string User { get => $"{BaseAPIUrl}User"; }
        public static string UserList { get => $"{BaseAPIUrl}User/UserList"; }
        public static string UserSearch { get => $"{BaseAPIUrl}User/UserSearch"; }
        public static string UserCompanies { get => $"{BaseAPIUrl}User/Companies"; }

        public static string User_Register = $"{BaseAddress}Identity/Register";

        public static string Account_Activate = $"{BaseAddress}Account/Activate";
        public static string MemberSearch { get => $"{BaseAPIUrl}Member/GlobalFind"; }
        public static string Identity_User { get => $"{BaseAddress}Identity/Users"; }
        public static string Identity_UserLogins { get => $"{BaseAddress}Identity/UsersLogins"; }
        public static string Identity_UserTokens { get => $"{BaseAddress}Identity/UserTokens"; }
        public static string Identity_UserRoles { get => $"{BaseAddress}Identity/UserRoles"; }
        public static string Identity_UserSystems { get => $"{BaseAddress}Identity/UserSystems"; }
        public static string Identity_Roles { get => $"{BaseAddress}Identity/Roles"; }
        public static string EvtProject { get => $"{BaseAPIUrl}Events/EvtProject"; }
        public static string EvtEvent { get => $"{BaseAPIUrl}Events/EvtEvent"; }
        public static string Events { get => $"{BaseAPIUrl}Events/EventSearch"; }
        public static string EventModify { get => $"{BaseAPIUrl}Events/EvtEvent"; }
        public static string EventAttend { get => $"{BaseAPIUrl}Events/EventAttend"; }
        public static string EventDetails { get => $"{BaseAPIUrl}Events/EventDetailSearch"; }
        public static string EventDetailScan { get => $"{BaseAPIUrl}Events/EventDetailScan"; }
        public static string EventProgramme { get => $"{BaseAPIUrl}Events/EventProgramme"; }
        public static string EventStatsComponent { get => $"{BaseAPIUrl}Events/EventStatsComponent"; }
        public static string EventComponent { get => $"{BaseAPIUrl}Events/EventComponents"; }
        public static string EventStatsEmpType { get => $"{BaseAPIUrl}Events/EventStatsEmpType"; }
        public static string Processors { get => $"{BaseAPIUrl}Processor"; }
        public static string ProcessorSchedules { get => $"{BaseAPIUrl}Processor/Schedules"; }
        public static string DepartmentSchedules { get => $"{BaseAddress}api/Department/Schedules"; }
        public static string ProcessorCompanies { get => $"{BaseAPIUrl}Processor/Companies"; }
        public static string ProcessorPendingRemittances { get => $"{BaseAPIUrl}Processor/PendingRemittances"; }
        public static string B2B_Remmittance { get => $"{BaseAPIUrl}B2B/Remmittance"; }
        public static string B2B_RemmittanceCapture { get => $"{BaseAPIUrl}B2B/CaptureImport"; }
        public static string B2B_RemmittanceByRemId { get => $"{BaseAPIUrl}B2B/RemmittanceByRemId"; }
        public static string B2B_RemittanceError { get => $"{BaseAPIUrl}B2B/RemittanceError"; }
        public static string B2B_RemmittanceDetail { get => $"{BaseAPIUrl}B2B/RemmittanceDetail"; }
        public static string B2B_RemmittanceDBDetailUpdate { get => $"{BaseAPIUrl}B2B/RemittanceDBDetails"; }



        public static string EmployeeDetails { get => $"{BaseAPIUrl}Employee/Details"; }
        public static string UserEmployee { get => $"{BaseAPIUrl}Employee/UserEmployee"; }
        public static string EmployeeSearch { get => $"{BaseAPIUrl}Employee/GlobalFind"; }
        public static string SystemMenuRights { get => $"{BaseAddress}System/MenuRights"; }
        //$"{BaseAPIUrl}Employee/UserEmployee"; }


        public static string ReportDocumentDownloadURL { get => $"{BaseAPIUrl}Document/ReportDownload"; }
        public static string DocumentFileDownloadURL { get => $"{BaseAPIUrl}Document/FileNameDownload"; }
        public static string DocumentFileIDDownloadURL { get => $"{BaseAPIUrl}Document/FileIDDownload"; }
        public static string DocumentFileUploadURL { get => $"{BaseAPIUrl}Document/FileUpload"; }

        public static string TransactionPaymentProcessMembers { get => $"{BaseAddress}Transaction/Payment/ProcessMembers"; }
        public static string TransactionPaymentUpdateMembers { get => $"{BaseAddress}Transaction/Payment/UpdateMembers"; }
        public static string TransactionPaymentGenerate { get => $"{BaseAddress}Transaction/Payment/Generate"; }
        public static string TransactionPaymentDelete { get => $"{BaseAddress}Transaction/Payment/Delete"; }
        public static string TransactionPaymentConfirm { get => $"{BaseAddress}Transaction/Payment/Confirm"; }
        public static string TransactionGetPayment { get => $"{BaseAddress}Transaction/Payment"; }

        public static string LegalCases { get => $"{BaseAPIUrl}Legal/Cases"; }


        public static string MST_Cities { get => $"{BaseAPIUrl}MasterData/City"; }
        public static string MST_Unions { get => $"{BaseAPIUrl}MasterData/Union"; }
        public static string MST_UnionSys { get => $"{BaseAPIUrl}MasterData/UnionSys"; }
        public static string MST_Status { get => $"{BaseAPIUrl}MasterData/Status"; }
        public static string MST_CaseType { get => $"{BaseAPIUrl}MasterData/CaseType"; }

        public static string RPT_FinanceMonthly { get => $"{BaseAPIUrl}Reporting/FinanceMonthly"; }
        public static string RPT_ListMonthlyReports { get => $"{BaseAPIUrl}Reporting/List/MonthlyReports"; }
        public static string ProcessTask_Delete { get => $"{BaseAddress}Task/TaskProcessRemove"; }
        public static string ProcessTask_Complete { get => $"{BaseAddress}Task/TaskProcessComplete"; }
        public static string ProcessTask_Start { get => $"{BaseAddress}Task/TaskProcessStart"; }
        public static string ProcessTask_Queue { get => $"{BaseAddress}Task/TaskProcessQueue"; }
        public static string ProcessTask_CheckStatus { get => $"{BaseAddress}Task/TaskProcessCheck"; }
        public static string DBTask_MemberUpdateGender { get => $"{BaseAPIUrl}DBTasks/MassUpdate_Gender"; }
        public static string DBTask_MemberUpdateAge { get => $"{BaseAPIUrl}DBTasks/MassUpdate_Age"; }
        public static string DBTask_MemberUpdateStatus { get => $"{BaseAPIUrl}DBTasks/MassUpdate_MemberStatus"; }




    }
}
