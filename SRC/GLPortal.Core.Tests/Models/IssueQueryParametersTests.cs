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
        var parameters = new IssueQueryParameters
        {
            State = IssueState.Opened,
            Labels = "bug,high priority",
            PerPage = 10
        };

        // Act 
        var result = parameters.ToString();

        // Assert
        result.Should().Be("state=opened&labels=bug%2Chigh%20priority&per_page=10");
    }
}
