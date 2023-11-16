using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Changes.Request;

public class ChangesRequestByLimit : ChangesRequest
{
    [Required]
    public int Limit { get; set; }
}
