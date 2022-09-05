namespace ClozerWoods.Models.Entities;

public class Page {
    public uint Id { get; init; }
    public uint? ParentId { get; set; }
    public string Stub { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public bool IsHome { get; set; }
    public bool Published { get; set; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; set; }
    public Page? Parent { get; set; }
    public IEnumerable<Page> Children { get; set; }
}
