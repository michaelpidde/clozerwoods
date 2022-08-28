using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ClozerWoods.Services;

public class MainGateService {
    public string UniqueString() => Convert.ToBase64String(Guid.NewGuid().ToByteArray())
        .Replace("-", "")
        .Replace("+", "")
        .Replace("=", "")
        .Replace("/", "");

    public string GenerateThumbnail(FileStream stream, string mediaFolder, string fileName) {
        const int width = 300;
        const int height = 250;

        // TODO: Rewrite this to ImageSharp or something before .NET 7

        #pragma warning disable CA1416 // Validate platform compatibility

        using var image = new Bitmap(stream);
        var pageUnit = GraphicsUnit.Pixel;
        var bounds = image.GetBounds(ref pageUnit);
        char sep = Path.DirectorySeparatorChar;
        if(bounds.Width < width && bounds.Height < height) {
            return fileName;
        }

        const string thumbFolder = "thumbs";
        string fullFolder = mediaFolder + sep + "thumbs";
        if(!Directory.Exists(fullFolder)) {
            try {
                Directory.CreateDirectory(fullFolder);
            } catch(Exception) {
                // TODO: Handle this gracefully
                throw new Exception("Thumbnail folder does not exist. Unable to create it.");
            }
        }

        // If image is rotated we need to fix it
        // ID for orientation is gleaned from here:
        // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.imaging.propertyitem.id?view=dotnet-plat-ext-6.0#system-drawing-imaging-propertyitem-id
        int orientationId = 0x0112;
        if(image.PropertyIdList.Contains(orientationId)) {
            var orientation = image.GetPropertyItem(orientationId);
            var flipType = RotateFlipType.RotateNoneFlipNone;
            switch(orientation!.Value![0]) {
                case 1:
                default:
                    flipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    flipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    flipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    flipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    flipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    flipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    flipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    flipType = RotateFlipType.Rotate270FlipNone;
                    break;
            }
            if(flipType != RotateFlipType.RotateNoneFlipNone) {
                // Rotate given the same flip type to undo it
                image.RotateFlip(flipType);
                image.RemovePropertyItem(orientationId);
                // Reset bounds or we'll have a squashed image
                bounds = image.GetBounds(ref pageUnit);
            }
        }

        var ratioX = (double)width / bounds.Width;
        var ratioY = (double)height / bounds.Height;
        var ratio = Math.Min(ratioX, ratioY);

        var thumb = new Bitmap((int)(bounds.Width * ratio), (int)(bounds.Height * ratio));
        using var graphics = Graphics.FromImage(thumb);
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.DrawImage(image, 0, 0, thumb.Width, thumb.Height);
        string filePath = fullFolder + sep + fileName;

        var extension = Path.GetExtension(filePath);
        ImageFormat format = ImageFormat.Png;
        switch(extension) {
            case ".gif":
                format = ImageFormat.Gif;
                break;
            case ".jpg":
            case ".jpeg":
                format = ImageFormat.Jpeg;
                break;
        }

        thumb.Save(filePath, format);

        #pragma warning restore CA1416 // Validate platform compatibility

        return thumbFolder + sep + fileName;
    }
}
