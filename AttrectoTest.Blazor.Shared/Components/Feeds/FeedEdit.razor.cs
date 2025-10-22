using AttrectoTest.Blazor.Common.Contracts;
using AttrectoTest.Blazor.Common.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AttrectoTest.Blazor.Common.Components.Feeds;

public partial class FeedEdit
{
    [Parameter]
    public EventCallback OnCloseModal { get; set;}

    [Inject]
    private IFeedService FeedService { get; set;}

    private FeedEditModel formData = new();
    private IBrowserFile? UploadedImage;

    private async Task HandleImageUpload(InputFileChangeEventArgs e)
    {
        UploadedImage = e.File;

        // Opcionálisan lekicsinyítheted a képet, ha túl nagy:
        var resizedFile = await UploadedImage.RequestImageFileAsync("image/png", 300, 300);
        var buffer = new byte[resizedFile.Size];
        await resizedFile.OpenReadStream().ReadAsync(buffer);

        // Preview base64 formátumban
        formData.ImageUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
        formData.ImageContent = buffer;
        formData.ImageFileName = e.File.Name;
    }

    async void HandleValidSubmit()
    {
        await FeedService.AddFeed(formData);
        await OnCloseModal.InvokeAsync();
    }

}
