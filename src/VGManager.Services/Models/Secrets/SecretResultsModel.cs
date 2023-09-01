using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGManager.Repository.Entities;

namespace VGManager.Services.Models.Secrets;
public class SecretResultsModel
{
    public Status Status { get; set; }
    public IEnumerable<SecretResultModel> Results { get; set; } = null!;
}
