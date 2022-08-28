using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.ViewModels.MainGate;

public class ListGalleriesViewModel : SharedViewModel {
    public IEnumerable<Gallery>? GalleryList { get; init; }
    public bool Modified { get; init; }
}
