using ImageMagick;      // Magick.NET
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Assortment.Web.Services
{
    public class ImageProcessingService
    {
        private const int ThumbSize = 200;
        private const int LargeMaxSide = 1500;

        public async Task<(byte[] Thumbnail, byte[] Large, int LargeWidth, int LargeHeight)> ProcessAsync(
            Stream input, CancellationToken ct = default)
        {  
            // 1. Конвертация HEIC/HEIF в JPEG через Magick.NET (с учётом EXIF-поворота)
            using var magick = new MagickImage(input);
            magick.AutoOrient(); // iPhone хранит поворот в EXIF, без этого фото будет боком[reference:2]
            magick.Format = MagickFormat.Jpg;
            magick.Quality = 92;

            using var jpegStream = new MemoryStream();
            await magick.WriteAsync(jpegStream, ct);
            jpegStream.Position = 0;

            // 2. Дальнейшая обработка через ImageSharp
            using var image = await Image.LoadAsync(jpegStream, ct);

            // 2.1. Миниатюра 200x200 (ресайз по короткой стороне + кроп по центру)
            using var thumb = image.Clone(ctx => ctx
                .Resize(new ResizeOptions
                {
                    Size = new Size(ThumbSize, ThumbSize),
                    Mode = ResizeMode.Crop, // короткая сторона → 200, лишнее обрезается[reference:3]
                    Position = AnchorPositionMode.Center
                }));

            // 2.2. Большое фото: пропорционально до 1500px по большей стороне (без растягивания маленьких)
            using var large = image.Clone(ctx =>
            {
                var (w, h) = (ctx.GetCurrentSize().Width, ctx.GetCurrentSize().Height);
                if (Math.Max(w, h) > LargeMaxSide)
                {
                    if (w >= h) ctx.Resize(LargeMaxSide, 0);
                    else ctx.Resize(0, LargeMaxSide);
                }
            });

            var thumbBytes = await EncodeJpegAsync(thumb, ct);
            var largeBytes = await EncodeJpegAsync(large, ct);
            var size = large.Size;

            return (thumbBytes, largeBytes, size.Width, size.Height);
        }

        private static async Task<byte[]> EncodeJpegAsync(Image image, CancellationToken ct)
        {
            using var ms = new MemoryStream();
            await image.SaveAsJpegAsync(ms, new JpegEncoder { Quality = 85 }, ct);
            return ms.ToArray();
        }
    }
    
}
