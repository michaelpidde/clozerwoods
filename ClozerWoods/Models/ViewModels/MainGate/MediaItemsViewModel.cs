using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.ViewModels.MainGate;

public class MediaItemsViewModel : SharedViewModel {
    public string MediaUrl { get; init; }
    public MediaItem? SelectedMediaItem { get; init; }
    public IEnumerable<MediaItem>? MediaItemList { get; init; }
    public IEnumerable<SelectListItem>? GalleryList { get; init; }
    public bool Modified { get; init; }
}
