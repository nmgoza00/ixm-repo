using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace IXM.Models.Events
{
    public class TPROJECTWr
    {
        [Key]
        public long PRJID { get; set; }
        public long SYSTEMID { get; set; }

        public int PRJTYPID { get; set; }
        public long PPRJID { get; set; }    // Parent Project
        public int STATUSID { get; set; }
        public string PRJCODE { get; set; } = null!;
        public string? DESCRIPTION { get; set; }
        public string? VENUE { get; set; }
        public string? RESPONSIBLEUNIT { get; set; }
        public string? SCOPE { get; set; }
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

    }

    public class TEVENTWr
    {

        [Key]
        public int SYSTEMID { get; set; }
        public int EVTID { get; set; }
        public int? PRJID { get; set; }
        public int? PERIODID { get; set; }
        public int? VENUEID { get; set; }
        public int? STATUSID { get; set; }
        public int EVTYPEID { get; set; }     // Event Type
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
        public string? EVENTURL { get; set; }
        public string? EVENTHTML { get; set; }      // sanitized HTML
        public string? LOCATIONAME { get; set; }
        public string? LOCATIONLINK { get; set; }
        public double LATITUDE { get; set; }
        public double LONGITUDE { get; set; }
        public DateTime? INSDAT { get; set; }
        public string? INSBY { get; set; }
        public DateTime? MODAT { get; set; }
        public string? MODBY { get; set; }
        public string? ISACTIVE { get; set; }   // Is Active Y/N
    }
    public class TPRJEVTATTACHMENTWr
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



    public class LocationMap
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GeoPosition { get; set; }
        public LocationMap(string text, double latitude, double longitude)
        {

            GeoPosition = GetGeoPositionFrommCoords(latitude, longitude);
            LocationName = text;
            Latitude = latitude;
            Longitude = longitude;

        }

        public static string GetGeoPositionFrommCoords(double latitude, double longitude)
        {
            return latitude.ToString("0.####") + ", " + longitude.ToString("0.####");
        }

    }
}
