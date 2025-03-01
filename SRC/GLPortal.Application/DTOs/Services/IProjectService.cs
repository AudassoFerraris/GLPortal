using GLPortal.Application.DTOs;

namespace GLPortal.Application.Services;

public interface IProjectService
{
    Task<List<ProjectSummaryDTO>> GetProjectsAsync();
}
