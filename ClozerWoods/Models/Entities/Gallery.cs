namespace ClozerWoods.Models.Entities;

public class Gallery {
    public uint Id { get; init; }
    public string Title { get; set; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; set; }
    public ICollection<MediaItem> MediaItems { get; set; }
}
