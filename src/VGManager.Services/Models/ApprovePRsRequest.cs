using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Services.Models;

public class ApprovePRsRequest
{
    public required Dictionary<string, int> PullRequests { get; set; }
    public required string Approver { get; set; }
    public required string ApproverId { get; set; }
}
