namespace GLPortal.Application.DTOs;

public class ProjectSummaryDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WebUrl { get; set; } = string.Empty;
    public int OpenIssues { get; set; }
    public int OpenLastMonth { get; set; }

    public IList<LabelDTO> Labels { get; set; } = new List<LabelDTO>();

    public IList<LabelDTO> PriorityLabels { get; set; } = new List<LabelDTO>();

    public IList<LabelDTO> CustomerLabels { get; set; } = new List<LabelDTO>();

}
