using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Repository.Entities;
public class SecretEntity
{
    public Status Status { get; set; }
    public KeyVaultSecret? Secret { get; set; } = null!;
}
