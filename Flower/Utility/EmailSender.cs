
using Mailjet;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flower.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public MailJetSettings mailJetSettings { get; set; }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string body)
        {
            mailJetSettings = configuration.GetSection("Mailjet").Get<MailJetSettings>();
            MailjetClient client = new(mailJetSettings.ApiKey, mailJetSettings.SecretKey) { };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
           .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "yyyboo@proton.me.com"},
        {"Name", "Boo"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          ".NET"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
      
       "HTMLPart",
             body  
         }
     }
           });
            await client.PostAsync(request);
        }
    }

}