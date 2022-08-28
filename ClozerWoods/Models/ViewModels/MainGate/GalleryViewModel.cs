using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.ViewModels.MainGate;

public class GalleryViewModel : SharedViewModel {
    public Gallery? SelectedGallery { get; init; }
    public IEnumerable<SelectListItem>? GalleryList { get; init; }

    public bool Add { get; init; }
}
