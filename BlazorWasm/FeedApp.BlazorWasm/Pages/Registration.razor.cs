using FeedApp.BlazorWasm.Services.IamBase;

using Microsoft.AspNetCore.Components;

using System.Net.Http.Json;

namespace FeedApp.BlazorWasm.Pages;

public partial class Registration
{
    private RegisterRequest registerModel = new();
    private string? message;
    private string? messageClass;

    [Inject]
    protected IAuthClient AuthClient { get; set; }

    private async Task HandleValidSubmit()
    {
        try
        {
            var response = await AuthClient.RegisterAsync(registerModel);

            if (!string.IsNullOrEmpty(response))
            {
                message = "Succesfull registration.";
                messageClass = "text-green-600";
                await Task.Delay(2000);
                Navigation.NavigateTo("/login");
            }
            //else
            //{
            //    var error = await response.Content.ReadAsStringAsync();
            //    message = $"Errort: {error}";
            //    messageClass = "text-red-600";
            //}
        }
        catch (Exception ex)
        {
            message = $"Client erro: {ex.Message}";
            messageClass = "text-red-600";
        }
    }
}
