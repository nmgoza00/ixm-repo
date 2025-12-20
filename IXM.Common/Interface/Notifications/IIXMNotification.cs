using IXM.Models;
using IXM.Constants;

namespace IXM.Common.Notification

{
    public interface IIXMNotification
    {

        public Task SendEmailAsync(IXMMailType pMailType, ref EMAIL_TEMPLATE email_Template);
        public Task SendSMSAsync(EMAIL_TEMPLATE email_Template);

    }
}
