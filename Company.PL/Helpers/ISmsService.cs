using Company.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Helpers
{
	public interface ISmsService
	{
		MessageResource Send(SmsMessage sms); 
	}
}
