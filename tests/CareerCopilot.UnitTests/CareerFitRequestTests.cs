using CareerCopilot.Application.UseCases.AnalyzeCareerFit;

namespace CareerCopilot.UnitTests;

public class CareerFitRequestTests
{
    [Fact]
    public void New_Request_Has_Empty_Default_Values()
    {
        var request = new CareerFitRequest();

        Assert.Equal(string.Empty, request.JobDescription);
        Assert.Equal(string.Empty, request.ResumeText);
    }
}