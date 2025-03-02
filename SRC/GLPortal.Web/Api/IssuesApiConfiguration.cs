using GLPortal.Core.Models;
using GLPortal.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GLPortal.Web.Api;

public static class IssuesApiConfiguration
{
	public static void ConfigureIssuesApi(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("toExcel", async (
			IProjectService projectService, 
			[AsParameters] IssueQueryParameters parameters) =>
		{
			var fileContent = await projectService.ExportIssuesToExcelAsync(parameters);
			return Results.File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "issues.xlsx");
		});
	}
}
