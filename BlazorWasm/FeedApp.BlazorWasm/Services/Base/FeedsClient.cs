using System.Text.Json;

namespace AttrectoTest.BlazorWasm.Services.Base;

public partial class FeedsClient : IFeedsClient
{
    public HttpClient HttpClient => _httpClient;

}
