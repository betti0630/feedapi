using AttrectoTest.BlazorWasm.Services.Base;

namespace AttrectoTest.BlazorWasm.Services.Base;

internal class BaseHttpService
{
    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
    {
        if (ex.StatusCode == 400)
        {
            return new Response<Guid>() { Message = "Invalid data was submitted", ValidationErrors = ex.Response, Success = false };
        }
        else if (ex.StatusCode == 404)
        {
            return new Response<Guid>() { Message = "The record was not found.", Success = false };
        }
        else
        {
            return new Response<Guid>() { Message = "Something went wrong, please try again later.", Success = false };
        }
    }
}
