using IXM.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using IXM.Constants;

namespace IXM.Common.Notification
{
    public class IXMNotification : IIXMNotification
    {



        private IWebHostEnvironment _environment;
        private IConfiguration _configuration;
        private readonly ILogger<IXMNotification> _logger;
        public IXMNotification(IWebHostEnvironment environment, 
            IConfiguration configuration,
             ILogger<IXMNotification> logger) 
        {
            this._environment = environment;
            this._configuration = configuration;
            this._logger = logger;
        }

        public class UniqueConstraintViolationException : Exception
        {
            public UniqueConstraintViolationException(string message, Exception inner) : base(message, inner) { }
        }

        private string GetBodyTemplate(IXMMailType pMailType)
        {

            string body = string.Empty;
            string path = string.Empty;
            if (pMailType == IXMMailType.Register)
            {
                path = Path.Combine(_environment.WebRootPath, "AppTemplates\\Email\\Register.html");
            } else if (pMailType == IXMMailType.ForgotPassword)
            {
                path = Path.Combine(_environment.WebRootPath, "AppTemplates\\Email\\ForgotPassword.html");
            } else if (pMailType == IXMMailType.AccountActivated)
            {
                path = Path.Combine(_environment.WebRootPath, "AppTemplates\\Email\\AccountActivated.html");
            }
            else if (pMailType == IXMMailType.PasswordChanged)
            {
                path = Path.Combine(_environment.WebRootPath, "AppTemplates\\Email\\PasswordChanged.html");
            }
            else if (pMailType == IXMMailType.RemmittanceReceived)
            {
                path = Path.Combine(_environment.WebRootPath, "AppTemplates\\Email\\RemmittnanceReceived.html");
            }
            else if (pMailType == IXMMailType.RemmittanceSubmitted)
            {
                path = Path.Combine(_environment.WebRootPath, "AppTemplates\\Email\\RemmittnanceSubmitted.html");
            }
            else if (pMailType == IXMMailType.CompanyRegistration)
            {
                path = Path.Combine(_environment.WebRootPath, "AppTemplates\\Email\\CompanyRegistration.html");
            }
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }

            return body;

        }

        private string PopulateBody(string body, EMAIL_TEMPLATE email_Template)
        {
            body = body.Replace("{Title}", "");
            body = body.Replace("{Logo}", _environment.ContentRootPath + "\\ixm\\ixm.ico");
            body = body.Replace("{Url}", "");
            body = body.Replace("{Description}", "");
            body = body.Replace("{Organization}", email_Template.Organization);
            body = body.Replace("{Username}", email_Template.userName);
            body = body.Replace("{Password}", email_Template.passWord);
            body = body.Replace("{Name}", email_Template.Name);
            body = body.Replace("{Surname}", email_Template.Surname);
            body = body.Replace("{VerifyLink}", email_Template.VerifyLink);
            body = body.Replace("{Message}", email_Template.Message);
            body = body.Replace("{activationLink}", email_Template.FreeText1);
            body = body.Replace("{Year}", email_Template.FreeText2);
            return body;
        }

        public async Task SendSMSAsync(EMAIL_TEMPLATE email_Template)
        {



            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://api.bulksms.com/v1/messages") };
            var authenticationBytes = Encoding.ASCII.GetBytes("nmgoza00:Nhlanhla1");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationBytes));


            string strbody = PopulateBody(email_Template.Message, email_Template);


                    var smessage = new List<SSMS_MESSAGE>()
                    {
                        new() {
                        to = email_Template.CellNumber,
                        body = strbody
                        }

                    };


            if (strbody != null) 
            {
            string json = JsonSerializer.Serialize<List<SSMS_MESSAGE>>(smessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var _ub = new UriBuilder(httpClient.BaseAddress);

            HttpResponseMessage response = await httpClient.PostAsync(_ub.Uri, content);
               
            } 

        }

        public Task SendEmailAsync(IXMMailType pMailType, ref EMAIL_TEMPLATE email_Template)
        {
            try
            {

                var mailuser = _configuration["Smtp:UserName"];
                var fromaddress = _configuration["Smtp:FromAddress"];
                var mailpass = _configuration["Smtp:Password"]; 
                int port = _configuration.GetValue<int>("Smtp:Port");
                var host = _configuration["Smtp:Server"];
                //"8NdRb{U1&eR(";
                //var mailuser = "ixmmobi@mikali.co.za";
                //var mailpass = "0@,ydNYni~5w";

                var body = string.Empty;

                if (pMailType == IXMMailType.Register)
                {
                    body = GetBodyTemplate(pMailType);
                } else if (pMailType == IXMMailType.ForgotPassword)
                {
                    body = GetBodyTemplate(pMailType);
                }
                else if (pMailType == IXMMailType.PasswordChanged)
                {
                    body = GetBodyTemplate(pMailType);
                }
                else
                {
                    body = GetBodyTemplate(pMailType);
                }


                //email_Template.Organization = "NUM";
                body = PopulateBody(body, email_Template);
                var mailmessage = new MailMessage(mailuser, email_Template.ToEmail);
                mailmessage.Subject = email_Template.Subject;
                mailmessage.Body = body;
                //mailmessage.BodyEncoding = Encoding.UTF8;
                mailmessage.IsBodyHtml = true;
                
                var mailclient = new SmtpClient(host, port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,                   
                    Port = port,
                    Credentials = new NetworkCredential(mailuser, mailpass)
                };

                mailclient.Send(mailmessage);
                _logger.LogInformation(email_Template.ToString());
                return Task.FromResult(email_Template.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromException(ex);
            }
        }
    }

}