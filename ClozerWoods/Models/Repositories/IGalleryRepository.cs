using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.Repositories;

public interface IGalleryRepository {
    IEnumerable<Gallery> Galleries { get; }
    Gallery Get(uint id);
    Gallery Get(string title);
    IEnumerable<SelectListItem> GetForSelect(uint? galleryId = null, string? defaultItemLabel = "* New");
    Gallery Add(Gallery gallery);
    Gallery Update(Gallery gallery);
}
