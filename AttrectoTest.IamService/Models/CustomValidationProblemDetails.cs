using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.IamService.Models;

internal class CustomProblemDetails : ProblemDetails
{
    public IDictionary<string, string[]>? Errors { get; set; } = new Dictionary<string, string[]>();
}
