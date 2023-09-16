using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Services.Models.VariableGroups.Results;

public abstract class VariableGroupResultBaseModel
{
    public string Project { get; set; } = null!;
    public bool SecretVariableGroup { get; set; }
    public string VariableGroupName { get; set; } = null!;
    public string VariableGroupKey { get; set; } = null!;
}
