using Contracts;
using Entities.Models;
using FluentEmail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService;

public class EmailService : IEmailService
{
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public EmailService(IFluentEmailFactory fluentEmailFactory)
    {
        _fluentEmailFactory = fluentEmailFactory;
    }
    public async Task SendMultiple(List<EmailMetadata> emailsMetadata)
    {
        foreach (var emailMetadata in emailsMetadata)
        {
            await _fluentEmailFactory
                .Create()
                .To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .Body(emailMetadata.Body, isHtml:true)   
                .SendAsync();
        }
    }
}
