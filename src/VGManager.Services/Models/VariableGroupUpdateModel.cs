using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Services.Models;

public class VariableGroupUpdateModel: VariableGroupModel
{
    public string NewValue { get; set; } = null!;

}
