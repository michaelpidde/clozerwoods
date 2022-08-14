namespace ClozerWoods.Models.Entities;

public class Gallery {
    public int Id { get; init; }
    public string Title { get; init; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; set; }
}
