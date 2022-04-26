using System.ComponentModel.DataAnnotations;

namespace BlazorUI.Models;

public class MessageModel
{
    [Required]
    public string User { get; set; }
    
    [Required]
    public string Message { get; set; }
}
