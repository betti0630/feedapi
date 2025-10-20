using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.IamService.Models;

public class CustomProblemDetails : ProblemDetails
{
    public IDictionary<string, string[]>? Errors { get; set; } = new Dictionary<string, string[]>();
}
