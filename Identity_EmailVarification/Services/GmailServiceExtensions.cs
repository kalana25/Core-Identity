using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Identity_EmailVarification.Services
{
    public static class GmailServiceExtensions
    {
        public static IServiceCollection AddGmailService(this IServiceCollection service,Action<Gmail> options=default)
        {
            options = options ?? (opt => { });
            service.Configure(options);
            service.AddSingleton<IGmailService, GmailService>();
            return service;
        }
    }
}
