﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using PayhouseDragonFly.CORE.ConnectorClasses.Response;
using PayhouseDragonFly.CORE.ConnectorClasses.Response.BseResponse;
using PayhouseDragonFly.CORE.DTOs.EmaillDtos;
using PayhouseDragonFly.CORE.Models.Emails;
using PayhouseDragonFly.INFRASTRUCTURE.Services.IServiceCoreInterfaces.IEmailServices;
using System.Threading;

namespace PayhouseDragonFly.INFRASTRUCTURE.Services.ServiceCore.EmailService
{
    public class EmailServices : IEmailServices
    {

        private readonly ILogger<IEmailServices> _logger;
        private readonly EmailConfiguration _emailconfig;

        public EmailServices(IOptions<EmailConfiguration> emailconfig,
            ILogger<IEmailServices> logger
            )
        {
            _emailconfig = emailconfig.Value;
            _logger = logger;

        }

        public async Task<mailresponse> SendEmail(string mailText, string subject, string recipient)
        {
            try
            {
                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                email.To.Add(MailboxAddress.Parse(recipient));
                email.Subject = subject;
                var builder = new BodyBuilder { HtmlBody = mailText };
                email.Body = builder.ToMessageBody();
                var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort), SecureSocketOptions.StartTls);
                var results = smtp.AuthenticateAsync(_emailconfig.EmailFrom, _emailconfig.SmtpPass);

                var resped = await smtp.SendAsync(email);

                _logger.LogInformation(" logging reponse : ", resped);
                await smtp.DisconnectAsync(true);

                return new mailresponse(true, "successfully sent email");
            }

            catch (Exception ex)
            {
                return new mailresponse(false, ex.Message);
            }
        }

        public async Task<mailresponse> SenTestMail(emailbody emailvm)
        {
            try
            {
                var currentdate = DateTime.Now.ToString("dd/MM/yy");
                _logger.LogInformation($"_____________________1. email service started  at {DateTime.Now} _______________________________");
                var file = @"Templates/Email/sendtext.html";
                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                StreamReader str = new StreamReader(file);
                string MailText = await str.ReadToEndAsync();
                str.Close();
                MailText = MailText.Replace("verificationstring", _emailconfig.SmtpUser)
                    .Replace("user", _emailconfig.SmtpUser)
                 
                    .Replace("sentTime", Convert.ToString(DateTime.Now));
                var builder = new BodyBuilder { HtmlBody = MailText };
                email.Body = builder.ToMessageBody();
                email.To.Add(MailboxAddress.Parse(emailvm.ToEmail));
                email.Subject = "Payhouse test mail";
                using var smtp = new SmtpClient();
                smtp.Connect(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort), 
                    MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailconfig.EmailFrom, _emailconfig.SmtpPass);
                var resp = await smtp.SendAsync(email);
                _logger.LogInformation("____________________ 3 email sender links ________________________________");
                smtp.Disconnect(true);
                _logger.LogInformation("____________________ 4 email sender links ________________________________");

                return new mailresponse(true, "mail sent successfully");
            }
            catch (Exception ex)
            {

                return new mailresponse(false, ex.Message);
            }
        }
        public async Task<mailresponse> SendEmailOnRegistration(emailbody emailvm)
        {
            try
            {
                _logger.LogInformation($"_____________________1.  email on registration service started  at {DateTime.Now} _______________________________");
                var file = @"Templates/Email/EmailOnRegistration.html";
                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                StreamReader str = new StreamReader(file);
                string MailText = await str.ReadToEndAsync();
                str.Close();
                var datesent = String.Format("{0:dd/MM/yyyy}", DateTime.Now); 
                MailText = MailText.Replace("subject", emailvm.UserName)
                    .Replace("user", _emailconfig.SmtpUser)
                     .Replace("emailsentdate", datesent)
                    .Replace("payload", emailvm.PayLoad);
                var builder = new BodyBuilder { HtmlBody = MailText };
                email.Body = builder.ToMessageBody();
                email.To.Add(MailboxAddress.Parse(emailvm.ToEmail));
                email.Subject = "Successfull registration";
                using var smtp = new SmtpClient();
                smtp.Connect(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort),
                    MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailconfig.EmailFrom, _emailconfig.SmtpPass);
                var resp = await smtp.SendAsync(email);
                _logger.LogInformation("____________________ 3 email sender links ________________________________");
                smtp.Disconnect(true);
                _logger.LogInformation("____________________ 4 email sender links ________________________________");
                _logger.LogInformation($"Email on registration sent successfully {DateTime.Now}");
                return new mailresponse(true, "mail sent successfully");         
            }
            catch (Exception ex)
            {

                return new mailresponse(false, ex.Message);
            }
        }


        //send email on leave completion


        public async Task<mailresponse> SendEmailOnLeaveCompletion(EmailbodyOnLeaveEnd emailvm) 
        {
            try
            {
                _logger.LogInformation($"_____________________1.  email starts {DateTime.Now} _______________________________");
                var file = @"Templates/Email/emailonleaveend.html";
                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                StreamReader str = new StreamReader(file);
                string MailText = await str.ReadToEndAsync();
                str.Close();
                var datesent = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                MailText = MailText.Replace("subject", emailvm.UserName)
                    .Replace("user", _emailconfig.SmtpUser)
                    .Replace("Names",emailvm.Names)
                    .Replace("leaveEndDate",Convert.ToString(emailvm.LeaveEndDate));
                   
                var builder = new BodyBuilder { HtmlBody = MailText };
                email.Body = builder.ToMessageBody();
                email.To.Add(MailboxAddress.Parse(emailvm.ToEmail));
                email.Subject = "Successfull registration";
                using var smtp = new SmtpClient();
                smtp.Connect(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort),
                    MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailconfig.EmailFrom, _emailconfig.SmtpPass);
                var resp = await smtp.SendAsync(email);
                _logger.LogInformation("____________________ 3 email sender links ________________________________");
                smtp.Disconnect(true);
                _logger.LogInformation("____________________ 4 email sender links ________________________________");
                _logger.LogInformation($"Email on registration sent successfully {DateTime.Now}");
                return new mailresponse(true, "mail sent successfully");
            }
            catch (Exception ex)
            {

                return new mailresponse(false, ex.Message);
            }
        }


        //email on created user

        public async Task<mailresponse>EmailOnCreatedUser(EmailbodyOnCreatedUser usermailvm)
        {
            try
            {
                _logger.LogInformation($"_____________________1.  email on registration service started  at {DateTime.Now} _______________________________");
                var file = @"Templates/Email/emailon_new_User_Created.html";
                var email = new MimeMessage { Sender = MailboxAddress.Parse(_emailconfig.SmtpUser) };
                StreamReader str = new StreamReader(file);
                string MailText = await str.ReadToEndAsync();
                str.Close();
                var datesent =  DateTime.Now.ToString("dddd, dd MMMM yyyy");
                var datecreated=  usermailvm.CreatedDate.ToString("dddd, dd MMMM yyyy");
                MailText = MailText.Replace("subject", "Notification on User Creation")
                    .Replace("usermail", _emailconfig.SmtpUser)
                    .Replace("emailsentdate", datesent)
                    .Replace("payload", usermailvm.PayLoad)
                    .Replace("useremail", usermailvm.UserEmail)
                    .Replace("dateCreated", datecreated)
                    .Replace("admin_names", usermailvm.AdminNames);


                var builder = new BodyBuilder { HtmlBody = MailText };
                email.Body = builder.ToMessageBody();
                email.To.Add(MailboxAddress.Parse(usermailvm.ToEmail));
                email.Subject = "Notification on User Creation";
                using var smtp = new SmtpClient();
                smtp.Connect(_emailconfig.SmtpHost, Convert.ToInt32(_emailconfig.SmtpPort),
                    MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailconfig.EmailFrom, _emailconfig.SmtpPass);
                var resp = await smtp.SendAsync(email);
                _logger.LogInformation("____________________ 3 email sender links ________________________________");
                smtp.Disconnect(true);
                _logger.LogInformation("____________________ 4 email sender links ________________________________");
                _logger.LogInformation($"Email on registration sent successfully {DateTime.Now}");
                return new mailresponse(true, "mail sent successfully");
            }
            catch (Exception ex)
            {

                return new mailresponse(false, ex.Message);
            }
        }

    }

}


    

