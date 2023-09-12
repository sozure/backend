using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Services.Models.Secrets;
public class SecretCopyModel: SecretBaseModel
{
    public string FromKeyVault { get; set; } = null!;

    public string ToKeyVault { get; set; } = null!;

    public bool overrideSecret { get; set; }
}
