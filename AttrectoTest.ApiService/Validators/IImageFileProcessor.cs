namespace AttrectoTest.ApiService.Validators;

public interface IImageFileProcessor
{
    Task<byte[]> ValidateAndGetContentOfImage(IFormFile file, CancellationToken cancellationToken);
    Task<string> ValidateAndGetUrlOfImage(IFormFile file, CancellationToken cancellationToken);
}
