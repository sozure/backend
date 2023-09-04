using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Repository.Entities;
public class SecretsEntity
{
    public Status Status { get; set; }
    public IEnumerable<SecretEntity?> Secrets { get; set; } = null!;
}
