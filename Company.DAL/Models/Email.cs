using Microsoft.AspNetCore.Http;

namespace Company.DAL.Models
{
	public class Email
	{
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        IList<IFormFile>? attachments { get; set; } = null;
    }
}
