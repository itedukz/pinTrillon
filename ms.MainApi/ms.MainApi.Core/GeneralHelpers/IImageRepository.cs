using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ms.MainApi.Core.GeneralHelpers;

public interface IImageRepository
{
    Task<Stream> UploadImageFileAsync(IFormFile imageFile, string pathToSave, string fileName, CancellationToken cancellationToken);
    bool UploadImageSingle(Stream imageStream, string pathToSave, string fileName);
}

public class ImageRepository : IImageRepository
{
    public async Task<Stream> UploadImageFileAsync(IFormFile imageFile, string pathToSave, string fileName, CancellationToken cancellationToken)
    {
        var saveFilePath = Path.Combine(pathToSave, fileName);
        await using var stream = new FileStream(saveFilePath, FileMode.Create);
        await imageFile.CopyToAsync(stream, cancellationToken);

        return stream;
    }

    public bool UploadImageSingle(Stream imageStream, string pathToSave, string fileName)
    {
        try
        {
            using var img = Image.Load(imageStream);
            var width = img.Width;
            var imgLarge = width switch
            {
                > 5000 => img.Clone(x => x.Resize(img.Width / 6, img.Height / 6)),
                > 2500 and < 4999 => img.Clone(x => x.Resize(img.Width / 3, img.Height / 3)),
                > 1500 and < 2499 => img.Clone(x => x.Resize(img.Width / 2, img.Height / 2)),
                _ => img.Clone(x => x.Resize(img.Width, img.Height))
            };

            imgLarge?.SaveAsJpeg(Path.Combine(pathToSave, fileName));

            return true;
        }
        catch
        {
            return false;
        }
    }

}