

using IXM.Models;
using IXM.Constants;
using IXM.Models.Core;
namespace IXM.Common.Data
{


    public class CustomData
    {

        private readonly List<CST_IDTYPE> idtype =
        [

        new(){
        IDTYPEID = "ID",
        DESCRIPTION = "Identity Number"},

        new(){
        IDTYPEID = "PA",
        DESCRIPTION = "Passport"},

        new(){
        IDTYPEID = "CSTM",
        DESCRIPTION = "Customized Identification"},

        ];


        private readonly List<CST_SIMPLEMASTER> CaseAffecting =
        [

        new(){
        ID = 0,
        DESCRIPTION = "Entity"},

        new(){
        ID = 1,
        DESCRIPTION = "Member"},

        new(){
        ID = 2,
        DESCRIPTION = "Staff"},

        ];


        private readonly List<CST_MGENDER> gender =
        [

        new(){
        GENDER = "M",
        DESCRIPTION = "Male"},

        new(){
        GENDER = "F",
        DESCRIPTION = "Female"},

        new(){
        GENDER = "U",
        DESCRIPTION = "Unknown"},

        ];


        private readonly List<CST_MEMEXISTS> memExists =
        [

        new(){
        MEMEXISTS = 1,
        DESCRIPTION = "Yes"},

        new(){
        MEMEXISTS = 0,
        DESCRIPTION = "No"},

        ];

        private readonly List<CST_IDTYPE> mCasePriority =
        [

        new(){
        IDTYPEID = "0",
        DESCRIPTION = "Low"},

        new(){
        IDTYPEID = "1",
        DESCRIPTION = "Medium"},

        new(){
        IDTYPEID = "2",
        DESCRIPTION = "High"},

        ];
        private readonly List<CONNECT_ENVIROMENT> ConnectEnv =
        [

        new(){
            CEID = 1,
            DESCRIPTION = "Test Environment",
            PORT = "TEST"},

        new(){
            CEID = 2,
            DESCRIPTION = "Production Environment",
            PORT = "PROD"},

        new(){
            CEID = 3,
            DESCRIPTION = "Quality Environment",
            PORT = "QA" }

        ];

        private readonly List<MSYSTEMS> MSystems =
                 [  new(){
                        SYSTEMID = "1",
                        SYSTEMNAME = "NUM" },

                    new(){
                        SYSTEMID = "2",
                        SYSTEMNAME = "SAMWU" },

                    new(){
                        SYSTEMID = "3",
                        SYSTEMNAME = "NUMSA" },

                    new(){
                        SYSTEMID = "4",
                        SYSTEMNAME = "NBCCI" },
                    new(){
                        SYSTEMID = "5",
                        SYSTEMNAME = "POPCRU"},
                    new(){
                        SYSTEMID = "6",
                        SYSTEMNAME = "TIRISANO"},
                    new(){
                        SYSTEMID = "-99",
                        SYSTEMNAME = "NOSYSTEM"}

                 ];

        private readonly List<MCOMPTYP> comptyp =
             [

        new(){
        COMPTYPID = 1,
        DESCRIPTION = "Main Company"},

        new(){
        COMPTYPID = 2,
        DESCRIPTION = "SubCompany"},

        new(){
        COMPTYPID = 4,
        DESCRIPTION = "Branches"},

    ];

        private readonly List<MUSER_GROUP> usergrp =
             [

        new(){
        UGID = 15,
        DESCRIPTION = "Processor",
        UGCODE = "MB_G_PRO"},

        new (){
        UGID = 16,
        DESCRIPTION = "Organisor",
        UGCODE = "MB_G_ORG"},

        new(){
        UGID = 17,
        DESCRIPTION = "Normal Member",
        UGCODE = "MB_G_MEM"},

        new(){
        UGID = 18,
        DESCRIPTION = "Secretariat",
        UGCODE = "MB_G_SEC"},

    ];


        private readonly List<MPERIOD> yearSelect =
             [

        new(){
        PRID = 15,
        MIMONTH = "-1",
        FYEAR = "2024",
        FDESCRIPTION = "2024",
        FMONTH = "",
        FYEARMONTH = "2024 - 01",
        MYEARMONTH = "2024 - 01",
        MYEAR = "2024"},

        new (){
        PRID = 16,
        MIMONTH = "-1",
        FYEAR = "2024",
        FDESCRIPTION = "2024",
        FMONTH = "",
        FYEARMONTH = "2024 - 01",
        MYEARMONTH = "2024 - 01",
        MYEAR = "2023"},

        new(){
        PRID = 17,
        MIMONTH = "-1",
        FYEAR = "2024",
        FDESCRIPTION = "2024",
        FMONTH = "",
        FYEARMONTH = "2024 - 01",
        MYEARMONTH = "2024 - 01",
        MYEAR = "2022"},

        new(){
        PRID = 18,
        MIMONTH = "-1",
        FYEAR = "2024",
        FDESCRIPTION = "2024",
        FMONTH = "",
        FYEARMONTH = "2024 - 01",
        MYEARMONTH = "2024 - 01",
        MYEAR = "2021" },

    ];


        private readonly List<IXMDDType> matchfields =
             [

        new(){
        ID = IxmRemittanceMatchTypes.MATCH_COMPANYID.ToString(),
        Description = "by Match Company"},

        new(){
        ID = IxmRemittanceMatchTypes.MATCH_BCOMPANYID.ToString(),
        Description = "by Match Branch"},
        new(){
        ID = IxmRemittanceMatchTypes.MATCH_GENDER.ToString(),
        Description = "by Match Gender"},
        new(){
        ID = IxmRemittanceMatchTypes.MATCH_SURNAME.ToString(),
        Description = "by Match Surname"},
        new(){
        ID = IxmRemittanceMatchTypes.MATCH_NAME.ToString(),
        Description = "by Match Name"},
        new(){
        ID = IxmRemittanceMatchTypes.MATCH_IDNUMBER.ToString(),
        Description = "by Match ID Number"},
        new(){
        ID = IxmRemittanceMatchTypes.MATCH_MEMSTATUSID.ToString(),
        Description = "by Match Member Status"},
        new(){
        ID = IxmRemittanceMatchTypes.MATCH_EMPNUMBER.ToString(),
        Description = "by Match Employee Number"},
        new(){
        ID = IxmRemittanceMatchTypes.MATCH_CITYID.ToString(),
        Description = "by Match Location"},

        ];


        private readonly List<CST_CITYTYPE> mCITYTYPE =
        [

        new(){
        CITYTYPEID = "CI",
        DESCRIPTION = "City"},

        new(){
        CITYTYPEID = "TN",
        DESCRIPTION = "Town"},

        new(){
        CITYTYPEID = "SB",
        DESCRIPTION = "Suburb"},        

        ];
        private readonly List<CST_MARSTAT> mMARITALSTATUS =
        [

        new(){
        MARSTATID = "SIN",
        DESCRIPTION = "Single"},

        new(){
        MARSTATID = "WID",
        DESCRIPTION = "Widowed"},

        new(){
        MARSTATID = "DIV",
        DESCRIPTION = "Divorced"},

        new(){
        MARSTATID = "MAR",
        DESCRIPTION = "Married"},

        ];
        private readonly List<MLANGUAGE> mLANGUAGE =
        [

        new(){
        LANGUAGEID = 1,
        DESCRIPTION = "English"},

        new(){
        LANGUAGEID = 2,
        DESCRIPTION = "Zulu"},

        new(){
        LANGUAGEID = 3,
        DESCRIPTION = "Xhosa"},

        new(){
        LANGUAGEID = 4,
        DESCRIPTION = "Sotho"},

        new(){
        LANGUAGEID = 5,
        DESCRIPTION = "Tsonga"},

        new(){
        LANGUAGEID = 6,
        DESCRIPTION = "Sepedi"},

        new(){
        LANGUAGEID = 7,
        DESCRIPTION = "Afrikaans"},

        new(){
        LANGUAGEID = 8,
        DESCRIPTION = "Venda"},

        new(){
        LANGUAGEID = 9
            ,
        DESCRIPTION = "Tswana"},

        new(){
        LANGUAGEID = 10,
        DESCRIPTION = "Swait"},

        new(){
        LANGUAGEID = 11,
        DESCRIPTION = "Ndebele"},

        new(){
        LANGUAGEID = 12,
        DESCRIPTION = "Shona"},

        ];

        public readonly List<IXMDocTypeToObject> docTypeToObjects = [
        new() { docType = IxmAppDocumentType.DIGISIGNATURE, SOURCEOBJ = IxmAppCaptionObject.MUSER.ToString() , SOURCEFLD = "USERID" },
        new() { docType = IxmAppDocumentType.USERPHOTO, SOURCEOBJ = IxmAppCaptionObject.MUSER.ToString(), SOURCEFLD = "USERID" },
        new() { docType = IxmAppDocumentType.EVENT, SOURCEOBJ = IxmAppCaptionObject.TPRJEVTD.ToString(), SOURCEFLD = "EVTDID" },
        new() { docType = IxmAppDocumentType.EMPLOYEEPHOTO, SOURCEOBJ = IxmAppCaptionObject.MEMPOYEE.ToString(), SOURCEFLD = "EMPID" },
        new() { docType = IxmAppDocumentType.MEMAPPLICATION, SOURCEOBJ = IxmAppCaptionObject.MMEMBER.ToString(), SOURCEFLD = "MEMBERID" }
        ];



        private readonly List<TCAPTIONS> OrganisorCompanyCaption =
        [

        new(){
        FIELD_NAME = "CNAME"},

        new(){
        FIELD_NAME = "MMEMCOUNT"},

        ];



        public MPERIOD[] GetYears() => [.. yearSelect];
        public MCOMPTYP[] GetCompType() => [.. comptyp];
        public List<MSYSTEMS> GetSystems() => [.. MSystems];
        public CONNECT_ENVIROMENT[] GetEnvironment() => [.. ConnectEnv];
        public List<IXMDDType> GetRemittanceMatchTypes() => [.. matchfields];
        public List<CST_IDTYPE> GetIdentityTypes() => [.. idtype];
        public List<CST_MGENDER> GetGender() => [.. gender];
        public List<CST_MEMEXISTS> GetMemExists() => [.. memExists];
        public List<CST_IDTYPE> GetCasePriority() => [.. mCasePriority];
        public List<TCAPTIONS> GetOrganisorCompanyCaptions() => [.. OrganisorCompanyCaption];
        public List<CST_SIMPLEMASTER> GetCaseAffecting() => [.. CaseAffecting];
        public List<IXMDocTypeToObject> GetDocumentTypeToObject() => [.. docTypeToObjects];
        public List<CST_CITYTYPE> GetCityType() => [.. mCITYTYPE];
        public List<MLANGUAGE> GetLanguage() => [.. mLANGUAGE];
        public List<CST_MARSTAT> GetMaritalStatus() => [.. mMARITALSTATUS];

    }
}
