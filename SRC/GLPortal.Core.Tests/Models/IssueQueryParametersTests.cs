using FluentAssertions;

using GLPortal.Core.Enums;
using GLPortal.Core.Models;

namespace GLPortal.Core.Tests.Models;
public class IssueQueryParametersTests
{
    [Fact]
    public void ToString_ReturnsCorrectString()
    {
        // Arrange
        var projectId = 3;
        var parameters = new IssueQueryParameters(projectId)
        {
            State = IssueState.Opened,
            Labels = "bug,high priority",
            PerPage = 10
        };

        // Act 
        var result = parameters.ToString();

        // Assert
        result.Should().Be("projectId=3&state=opened&labels=bug%2Chigh%20priority&per_page=10");
    }
}
