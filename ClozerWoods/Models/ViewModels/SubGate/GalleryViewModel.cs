using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.ViewModels.SubGate;

public class GalleryViewModel: SharedViewModel {
    public Gallery? SelectedGallery { get; init; }
    public IEnumerable<SelectListItem>? GalleryListItems { get; init; }
}
