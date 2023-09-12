using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Services.Models.Secrets;
public class SecretModel: SecretBaseModel
{
    public string KeyVaultName { get; set; } = null!;
    public string SecretFilter { get; set; } = null!;
}
