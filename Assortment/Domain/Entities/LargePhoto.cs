namespace Assortment.Domain.Entities
{
    public class LargePhoto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ModelId { get; set; }
        public Model Model { get; set; } = null!;

        public byte[] Data { get; set; } = null!;
        public string ContentType { get; set; } = "image/jpeg";
        public int Width { get; set; }
        public int Height { get; set; }

        public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
