namespace ClozerWoods.Models.Entities;

public class GalleryItem {
    public int Id { get; init; }
    public int GalleryId { get; set; }
    public string FileName { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; set; }
}
