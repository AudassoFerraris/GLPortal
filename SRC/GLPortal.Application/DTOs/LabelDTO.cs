using GLPortal.Core.Models;

namespace GLPortal.Application.DTOs;
public class LabelDTO
{
    public LabelDTO()
    {
        
    }

    public LabelDTO(Label l)
    {
        Id = l.Id;
        Name = l.Name;
        Color = l.Color;
        Description = l.Description;
        Priority = l.Priority;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Name used for priority and customers labels
    /// calculated from the original name using the regex
    /// specified in the settings
    /// </summary>
    public string? SimpleName { get; set; }
    public string Color { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Priority { get; set; }
}
