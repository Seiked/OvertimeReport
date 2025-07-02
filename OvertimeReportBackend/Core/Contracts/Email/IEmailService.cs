using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Contracts.Email
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string toUser, string subject, string content);
    }
}