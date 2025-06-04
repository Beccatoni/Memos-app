using System.ComponentModel.DataAnnotations;

namespace MemosApplication.Api.DTOs;

public class CreateMemoDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set;}
    
    [Required]
    public string Content { get; set;}
}