namespace ClozerWoods.Models.Entities;

public class Page {
    public int Id { get; init; }
    public string Title { get; set; }
    public bool Published { get; set; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; set; }
}
