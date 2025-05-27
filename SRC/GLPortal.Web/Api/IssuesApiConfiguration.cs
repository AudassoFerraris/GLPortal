using GLPortal.Core.Models;
using GLPortal.Application.Services;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.InkML;

namespace GLPortal.Web.Api;

public static class IssuesApiConfiguration
{
	public static void ConfigureIssuesApi(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("toExcel", async (
			IProjectService projectService,
            HttpContext context) =>
        {

            var queryString = context.Request.QueryString.Value?.TrimStart('?');
            if (!IssueQueryParameters.TryParse(queryString, null, out var parameters))
            {
                return Results.BadRequest("Invalid query string");
            }


            var fileContent = await projectService.ExportIssuesToExcelAsync(parameters);
			return Results.File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "issues.xlsx");
		});
	}
}
