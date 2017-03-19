using SchedbotDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace SchedBot
{
    public class MailService
    {
        static bool mailSent = false;

        public async Task OnScheduleFinalized(object source, ScheduleFinalizeEventArgs args)
        {

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new NetworkCredential("schedbotuc@gmail.com", "bearcats");
            client.EnableSsl = true;

            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress("SchedBot@schedbot.com",
               "SchedBot ",
            System.Text.Encoding.UTF8);

            MailMessage message = new MailMessage();
            message.From = from;
            foreach (User user in args.Users)
            {
                MailAddress to = new MailAddress(user.Email);
                message.To.Add(to);

            }
            message.Body = String.Format("The Schedule for the week of {0} - {1} has been Finalized. Go to Schedbot.com to view the schedule.", args.Schedule.StartDate, args.Schedule.EndDate);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "Schedule Finalized";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            
            // The userState can be any object that allows your callback 
            // method to identify this send operation.
         
           await client.SendMailAsync(message);

            // Clean up.
            message.Dispose();
            client.Dispose();
            Console.WriteLine("Goodbye.");
        }
        
    }
}