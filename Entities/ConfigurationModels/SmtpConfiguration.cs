using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ConfigurationModels;

public class SmtpConfiguration
{
    public string Section { get; set; } = "SmtpSettings";

    public string? From { get; set; }
    public string? SmtpServer { get; set; }
    public string? Port { get; set; }
    public string? UserName { get; set; }

}
