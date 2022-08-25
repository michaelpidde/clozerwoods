namespace ClozerWoods.Models.Entities;

public class MediaItem {
    public uint Id { get; init; }
    public uint GalleryId { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; set; }
}
