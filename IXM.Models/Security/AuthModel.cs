using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IXM.Models
{
	public class AuthModel
	{
	}

	public class LoginModel
	{
		[Required(ErrorMessage = "Username is required.")]
		//[EmailAddress(ErrorMessage = "Email address is not valid.")]
		public string? email { get; set; } // NOTE: email will be the username, too

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		public string? password { get; set; }
        public string? loginrole { get; set; }
        public int? CEID { get; set; }
	}

	public class LoginResult
	{
        public string? id { get; set; }
		public string? message { get; set; }
		public string? email { get; set; }
		public string? systemusername { get; set; }
        public int systemuserid { get; set; }
        public string? systemname { get; set; }
        public string? password { get; set; }
        public List<USER_COMPANY>? companyid { get; set; }
        public int? CEID { get; set; }

        [DefaultValue("")]
        public List<ID_UserRoles> loginrole { get; set; }
        public string? jwtBearer { get; set; }
		public bool? success { get; set; }
		public string? accessToken { get; set; }
		public string? refreshToken { get; set; }
		public int expiresIn { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidTo { get; set; }
		public string? tokentype { get; set; }
	}

    public class DEVICE_INFO
    {
        public string Model { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string OSVersion { get; set; }
        public string Idiom { get; set; }
        public string Platform { get; set; }
        public string DeviceType { get; set; }
    }

    /// Mainly used by the Mobile Application
    /// Can look at seprating this to a seperate Class file 
    /// </summary>

    
    public class USER_TOKEN
    {
        public string tokenType { get; set; }
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string refreshToken { get; set; }
    }





}
