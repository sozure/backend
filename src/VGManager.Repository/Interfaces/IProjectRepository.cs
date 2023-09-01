using Microsoft.TeamFoundation.Core.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGManager.Repository.Interfaces;
public interface IProjectRepository
{
    Task<IEnumerable<TeamProjectReference>> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default);
}
