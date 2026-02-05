using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using YTU.EmailService;

namespace BusinessLayer.Services.EmailProcessing
{
    public class EmailHandlerService
    {
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmailHandlerService> _logger;
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly IEmailRepository _emailRepository;

        public EmailHandlerService(IEmailSender emailSender, IWebHostEnvironment webHostEnvironment, ILogger<EmailHandlerService> logger, IOptions<EmailSettings> emailSettings, IEmailRepository emailRepository)
        {
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _emailSettings = emailSettings;
            _emailRepository = emailRepository;
        }

        public async Task SendEmailAsync(Email email)
        {
            email.Subject = $"ChaReq - {email.Subject}";

            EmailResult emailResult = new();
            try
            {
                emailResult = await _emailSender.SendEmailAsync(email);
                LogEmail(email);
            }
            catch (Exception exception)
            {
                _logger.LogError($"{nameof(EmailHandlerService)}: ERROR SENDING {email.Subject}");
                _logger.LogError($"ERROR SENDING {email.Subject} {exception.Message}");
            }
        }

        public void LogEmail(Email email)
        {
            SentEmail sentEmail = new SentEmail
            {
                SentTo = email.ToAddresses,
                Subject = email.Subject,
                HtmlBody = email.Body,
                SentAt = DateTime.Now,
                BccEmails = email.BccAddresses,
                CcEmails = email.CcAddresses,
                ReplyTo = _emailSettings.Value.ReplyToEmail, 
                SentFrom = _emailSettings.Value.SenderEmail
                //Username =
            };
            _emailRepository.Insert(sentEmail);
            _emailRepository.Save();
        }
    }

}