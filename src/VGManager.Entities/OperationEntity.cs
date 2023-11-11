using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Entities;

public abstract class OperationEntity
{
    public string Id { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Organization { get; set; } = null!;
    public string Project { get; set; } = null!;
    public DateTime Date { get; set; }
    public string VariableGroupFilter { get; set; } = null!;
}
