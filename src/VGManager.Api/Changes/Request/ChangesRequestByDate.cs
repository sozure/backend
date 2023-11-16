using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Changes.Request;

public class ChangesRequestByDate: ChangesRequest
{
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }
}
