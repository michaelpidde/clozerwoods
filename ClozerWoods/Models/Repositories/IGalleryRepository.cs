using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.Repositories;

public interface IGalleryRepository {
    IEnumerable<Gallery> Galleries { get; }
    Gallery Get(uint id);
    Gallery Get(string title);
    Gallery Add(Gallery gallery);
    Gallery Update(Gallery gallery);
}
