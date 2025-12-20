using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IXM.Models.Events
{
        public class TPROJECT
        {
            [Key]
            public int PRJID { get; set; }
            public int SYSTEMID { get; set; }

            public int PRJTYPID { get; set; }
            public long PPRJID { get; set; }    // Parent Project
            public int STATUSID { get; set; }
            public string PRJCODE { get; set; } = null!;
            public string? DESCRIPTION { get; set; }
            [MaxLength(100, ErrorMessage = "Name is too long (50 character limit).")]
            public string? VENUE { get; set; }
            public string? RESPONSIBLEUNIT { get; set; }
            [MaxLength(100)] public string? SCOPE { get; set; }
            public double LATITUDE { get; set; }
            public double LONGITUDE { get; set; }
            public DateTime? PSTDAT { get; set; }    // UTC
            public DateTime? PENDAT { get; set; }      // UTC
            public DateTime INSDAT { get; set; } = DateTime.UtcNow;
            public int? EHCPP { get; set; }
            public int? ENOR { get; set; }
            public string INSBY { get; set; }
            public DateTime MODAT { get; set; } = DateTime.UtcNow;
            public string MODBY { get; set; }
            public string ISACTIVE { get; set; }

        // Navigation
            public ICollection<TEVENT> Events { get; set; } = new List<TEVENT>();
        }

        public class TEVENT
        {

            [Key]
            public int SYSTEMID { get; set; }
            public int EVTID { get; set; }
            public int? PRJID { get; set; }
            public int? PERIODID { get; set; }
            public int? VENUEID { get; set; }
            public int? STATUSID { get; set; }
            public int EVTYPEID { get; set; }     // Event Type

            [Required, MaxLength(200)]
            public string? DESCRIPTION { get; set; } = null!;
            public DateTime POSDAT { get; set; }     // UTC
            public DateTime PSTDAT { get; set; }   // UTC
            public DateTime PENDAT { get; set; }     // UTC

            public int? EHCPP { get; set; }  // Estimated Hourly Cost Per Person
            public int? ENOR { get; set; }   // Estimated Number of Resources
            public int? EXPA { get; set; }      //Expected Attendance
            public int? NOPE { get; set; }      //Expected Attendance
            public int? NOPR { get; set; }      //Expected Attendance
            public int? DWBA { get; set; }      //Expected Attendance
            public int? DCAR { get; set; }      //Expected Attendance
            public int? DNOT { get; set; }      //Expected Attendance

            [MaxLength(500)] public string? EVENTURL { get; set; }
            public string? EVENTHTML { get; set; }      // sanitized HTML
            [MaxLength(250)] public string? LOCATIONAME { get; set; }
            [MaxLength(250)] public string? LOCATIONLINK { get; set; }
            public double LATITUDE { get; set; }
            public double LONGITUDE { get; set; }
            public DateTime? INSDAT { get; set; } = DateTime.UtcNow;
            public string? INSBY { get; set; }
            public DateTime? MODAT { get; set; } = DateTime.UtcNow;
            public string? MODBY { get; set; }
            public string? ISACTIVE { get; set; }   // Is Active Y/N
            public ICollection<TPROJECT> Events { get; set; } = new List<TPROJECT>();
    }

    public class TPRJEVT
        {


            [Key]
            public int PEVTID { get; set; }
            public int? PRJID { get; set; }
            public TPROJECT Campaign { get; set; } = null!;

            public int? PERIODID { get; set; }
            public DateTime? INSDAT { get; set; }
            public string? INSBY { get; set; }
            public int? VENID { get; set; }
            public int STATUSID { get; set; }

            [Required, MaxLength(200)]
            public string? DESCRIPTION { get; set; } = null!;
            [MaxLength(200)] public string? Subject { get; set; }
            [MaxLength(100)] public string? ResponsibleUnit { get; set; }
            [MaxLength(100)] public string? Scope { get; set; }

            public DateTime PSTDAT { get; set; }   // UTC
            public DateTime PENDAT { get; set; }     // UTC

            public string? EHCPP { get; set; }
            public string? ENOR { get; set; }
            public int? ExpectedAttendance { get; set; }

            [MaxLength(250)] public string? LocationName { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            [MaxLength(500)] public string? MapLink { get; set; }

            [MaxLength(500)] public string? Url { get; set; }
            public string? PAGEHTML { get; set; }      // sanitized HTML

            [MaxLength(20)] public string Status { get; set; } = "Draft";
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime? UpdatedAt { get; set; }

            // Attachments navigation if needed
            public ICollection<TPRJEVTATTACHMENT> Attachments { get; set; } = new List<TPRJEVTATTACHMENT>();
        }
        public class TPRJEVTATTACHMENT
        {
            public long EVTATTACHMENTID { get; set; }
            public long EventId { get; set; }
            public TPRJEVT Event { get; set; } = null!;
            public string FileName { get; set; } = null!;
            public string ContentType { get; set; } = "application/octet-stream";
            public string StoragePath { get; set; } = null!; // remote path or file id
            public long SizeBytes { get; set; }
            public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        }



        public class MEVENT_COMPONENT
        {
            [Key]
            public int ECOMPID { get; set; }
            public string? DESCRIPTION { get; set; }
            public DateTime? MODAT { get; set; }
            public string? MODBY { get; set; }
        }

        public class CEVT_COMPONENT
        {
            [Key]
            public int ECOMPID { get; set; }
            public int PEVTID { get; set; }
            public int? MSGID { get; set; }
            public string? MSGACTIVE { get; set; }
            public string? ISACTIVE { get; set; }


        }

        public class VCEVT_COMPONENT
        {
            [Key]
            public int ECOMPID { get; set; }
            public int PEVTID { get; set; }
            public int? MSGID { get; set; }
            public string? MSGACTIVE { get; set; }
            public string? ISACTIVE { get; set; }
            public string? EVENT_DESCRIPTION { get; set; }
            public string? DESCRIPTION { get; set; }
            public string? SMESSAGE { get; set; }

        }

        public class TPRJEVT_DISPLAY
        {
            [Key]
            public int PEVTID { get; set; }
            public int? PRJID { get; set; }
            public string? DESCRIPTION { get; set; }
            public DateTime? PSTDAT { get; set; }
            public DateTime? PENDAT { get; set; }
            public string? EHCPP { get; set; }
            public string? ENOR { get; set; }
            public int? PERIODID { get; set; }
            public DateTime? INSDAT { get; set; }
            public string? INSBY { get; set; }
            public int? VENID { get; set; }
            public int? STATUSID { get; set; }
            public string? PRJ_DESCRIPTION { get; set; }
        }

        public class TPRJEVTD
        {
            [Key]
            public int PEVTDID { get; set; }
            public int PEVTID { get; set; }
            public string? SOURCEOBJ { get; set; }
            public string? SOURCEFLD { get; set; }
            public int SOURCEID { get; set; }
            public string? INSDAT { get; set; }
            public string? INSBY { get; set; }
            public string? BCODE { get; set; }
            public int? HATD { get; set; }
            public int STATUSID { get; set; }
            public int? SSMS { get; set; }
            public int? CLTEMPID { get; set; }
            public int? RPLEMPID { get; set; }
            public DateTime? MODAT { get; set; }
            public string? MODBY { get; set; }
            public string? EFFDATEF { get; set; }
            public string? EFFDATET { get; set; }
            public int? LT { get; set; }
        }


        public class TPRJEVTDD
        {
            [Key]
            public int PEVTDID { get; set; }
            public int STATUSID { get; set; }
            public int? ECOMPID { get; set; }
            public string? MACHINENAME { get; set; }
            public DateTime? INSDAT { get; set; }
            public string? INSBY { get; set; }

        }

        public class TPRJEVTD_DISPLAY
        {
            [Key]
            public int PEVTDID { get; set; }
            public int PEVTID { get; set; }
            public string? SOURCEOBJ { get; set; }
            public string? SOURCEFLD { get; set; }
            public int SOURCEID { get; set; }
            public DateTime? INSDAT { get; set; }
            public string? INSBY { get; set; }
            public string? BCODE { get; set; }
            public int? HATD { get; set; }
            public int? STATUSID { get; set; }
            public int? SSMS { get; set; }
            public int? CLTEMPID { get; set; }
            public int? RPLEMPID { get; set; }
            public DateTime? MODAT { get; set; }
            public string? MODBY { get; set; }
            public DateTime? EFFDATEF { get; set; }
            public DateTime? EFFDATET { get; set; }
            public int? LT { get; set; }

            public string? IDNUMBER { get; set; }
            public string? CELLNUMBER { get; set; }
            public string? MNAME { get; set; }
            public string? MSURNAME { get; set; }
            public string? EMPNUMBER { get; set; }
            public string? COMPANY_DNU { get; set; }
            public string? LOCALITY_D { get; set; }
            public string? UNION_DESCRIPTION { get; set; }
            public string? POSITION_DESCRIPTION { get; set; }
            public string? PRJ_DESCRIPTION { get; set; }
            public string? EMPTYP_DESCRIPTION { get; set; }
            public string? STATUS_DESCRIPTION { get; set; }
            public string? PDOCUMENTNAME { get; set; }
            public string? CODE_FPATH { get; set; }
            public int? OBJECTID { get; set; }

            [NotMapped]
            public string? DOCUMENT_URI { get; set; }

        }


        public class TPRJEVTS_DISPLAY
            {
        [Key]
        public int PEVTSID { get; set; }
        public int PEVTID { get; set; }
        public int? ECOMPID { get; set; }
        public string? SOURCEOBJ { get; set; }
        public string? SOURCEFLD { get; set; }
        public string? SOURCEID { get; set; }
        public DateTime? START_DATETIME { get; set; }
        public DateTime? END_DATETIME { get; set; }
        public string? NOTES { get; set; }
        public string? INSDAT { get; set; }
        public string? INSBY { get; set; }
        public string? MODAT { get; set; }
        public string? MODBY { get; set; }
        public int? LT { get; set; }
        //PRIMARILY FOR Mobile App
        public TimeOnly START_TIME
        {
            get { return TimeOnly.FromDateTime(START_DATETIME.Value); }
        }
        public TimeOnly END_TIME
        {
            get { return TimeOnly.FromDateTime(END_DATETIME.Value); }
        }
        public DateOnly EVENT_DAY
        {
            get { return DateOnly.FromDateTime(END_DATETIME.Value); }
        }
    }


    public class TPRJEVTS
        {
            [Key]
            public int PEVTSID { get; set; }
            public int PEVTID { get; set; }
            public int? ECOMPID { get; set; }
            public string? SOURCEOBJ { get; set; }
            public string? SOURCEFLD { get; set; }
            public string? SOURCEID { get; set; }
            public DateTime? START_DATETIME { get; set; }
            public DateTime? END_DATETIME { get; set; }
            public string? NOTES { get; set; }
            public DateTime? INSDAT { get; set; }
            public string? INSBY { get; set; }
            public DateTime? MODAT { get; set; }
            public string? MODBY { get; set; }
            public int? LT { get; set; }
        }
        public class REP_EVTSTATS_S
        {

            public int? PEVTID { get; set; }
            public string REPORT { get; set; }
            [Key]
            public int ECOMPID { get; set; }
            public string COMPONENT { get; set; }
            public int DELEGATES { get; set; }
        }


        public abstract class TPRJEVTSG : List<TPRJEVTS_DISPLAY>
        {
            public DateOnly EVENT_DAY { get; set; }
            public TPRJEVTSG(List<TPRJEVTS_DISPLAY> pevent) : base(pevent)
            {
                EVENT_DAY = pevent.FirstOrDefault().EVENT_DAY;
            }

        }
    public class EventScan
        {
            public string ScanCode { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string ImageLink { get; set; }
        }






}
