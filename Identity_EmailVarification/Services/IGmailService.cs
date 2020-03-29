using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_EmailVarification.Services
{
    public interface IGmailService
    {
        void SendAsync(string destinationEmail,string subject,string content);
    }
}
