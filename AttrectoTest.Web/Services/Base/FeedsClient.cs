using System.Text.Json;

namespace AttrectoTest.Web.Services.Base;

public partial class FeedsClient : IFeedsClient
{
    public HttpClient HttpClient => _httpClient;

}
