using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Web;

namespace FitnessPlatform.Models
{
    public class CSendEmail
    {
		public static void sendemail(string[] emailaddress, string emailbody)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("黃柏瑄", "asdfg919001@Gmail.com"));
			foreach (var items in emailaddress)
			{
				message.To.Add(new MailboxAddress("Gymuser用戶", items));
			}
			message.Subject = "How you doin'?";

			message.Body = new TextPart("html")
			{
				Text = emailbody
			};
			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587, false);

				// Note: only needed if the SMTP server requires authentication
				client.Authenticate("asdfg919001@Gmail.com", "a8508240dsa");

				client.Send(message);
				client.Disconnect(true);
			}
		}
		public static string getnum(string nums)
		{
			return string.Format(@"Hey Chandler,

						I just wanted to let you know that Monica and I were going to go play some paintball, you in?

						<a href='https://fitnessplatform20201006211746.azurewebsites.net/Member/CheckMember?fMember_Num={0}'>點此驗證跳轉</a>
							 -- Joey", nums);
		}
	}
}