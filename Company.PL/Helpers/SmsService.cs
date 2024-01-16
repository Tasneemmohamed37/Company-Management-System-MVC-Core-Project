using Company.DAL.Models;
using Company.PL.Settings;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Helpers
{
	public class SmsService:ISmsService
	{
		private readonly TwillioSettings options;

		public SmsService(IOptions<TwillioSettings> options)
        {
			this.options = options.Value;
		}

		public MessageResource Send(SmsMessage sms)
		{
			TwilioClient.Init(options.AccountSID, options.AuthToken);
			var result = MessageResource.Create(
				body: sms.Body,
				from: new Twilio.Types.PhoneNumber(options.TwilioPhoneNumber),
				to: sms.phoneNumber
				);
			return result;
		}
	}
}
