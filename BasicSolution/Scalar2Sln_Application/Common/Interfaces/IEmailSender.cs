using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Common.Interfaces
{
    public interface IEmailSender
    {
      public  void SendEmail(string name);
    }
}
