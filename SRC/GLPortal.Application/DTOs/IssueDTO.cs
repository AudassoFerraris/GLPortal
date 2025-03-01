using GLPortal.Core.Enums;
using GLPortal.Core.Models;

namespace GLPortal.Application.DTOs;
public class IssueDTO
{
    public IssueDTO(Issue source)
    {
        Iid = source.Iid;
        Title = source.Title;
        CreatedAt = source.CreatedAt;
        State = source.State;
        Assignees = source.Assignees?.Select(u => u.Username).ToArray();
    }

    public int Iid { get; set; }

    public string Title { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public IssueState State { get; set; }

    public string[]? Assignees { get; set; }
}
