using System.ComponentModel.DataAnnotations;

namespace MemosApplication.Api.DTOs;

public class UpdateMemoDto
{
    [StringLength(100)]
    public string? Title { get; set; }
    
    public string? Content { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
}