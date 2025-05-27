using Scalar2Sln_Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Infrastructure.Services
{
    public class EmailSender: IEmailSender
    {
        public EmailSender() { }

        public void SendEmail(string email)
        {

            Console.WriteLine($"记录发送邮件的测试{email}");
        }
    }
}
