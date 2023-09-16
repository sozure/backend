using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Services.Models.Secrets.Results;
public class SecretBaseResult
{
    public string KeyVault { get; set; } = null!;
    public string SecretName { get; set; } = null!;
    public string SecretValue { get; set; } = null!;
}
