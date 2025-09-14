using AttrectoTest.Application.Exceptions;

namespace AttrectoTest.ApiService.Validators;

internal class ImageFileProcessor: IImageFileProcessor
{
    public async Task<byte[]> ValidateAndGetContentOfImage(IFormFile file, CancellationToken cancellationToken) {
        if (file == null || file.Length == 0)
            throw new BadRequestException("No file uploaded.");

        if (file.Length > 5 * 1024 * 1024)
            throw new BadRequestException("File too large (max 5 MB).");

        var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExt.Contains(ext))
            throw new BadRequestException("Unsupported file extension.");

        // 3) Check MIME type
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        if (!allowedTypes.Contains(file.ContentType))
            throw new BadRequestException("Unsupported file type.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, cancellationToken);
        var bytes = ms.ToArray();

        // 5) Check magic number for JPEG/PNG/GIF
        if (ext == ".jpg" || ext == ".jpeg")
        {
            if (!(bytes[0] == 0xFF && bytes[1] == 0xD8))
                throw new BadRequestException("Invalid JPEG file.");
        }
        else if (ext == ".png")
        {
            if (!(bytes[0] == 0x89 && bytes[1] == 0x50))
                throw new BadRequestException("Invalid PNG file.");
        }
        return bytes;
    }
}
