using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGService.Model;

public class MatchedDeletedSecret
{
    public string SecretName { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }

    public MatchedDeletedSecret(string secretName, DateTimeOffset? deletedOn)
    {
        SecretName = secretName;
        DeletedOn = deletedOn;
    }
}
