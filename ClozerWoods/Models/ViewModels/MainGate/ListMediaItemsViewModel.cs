using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.ViewModels.MainGate;

public class ListMediaItemsViewModel : SharedViewModel {
    public string MediaUrl { get; init; }
    public IEnumerable<MediaItem>? MediaItemList { get; init; }
    public bool Modified { get; init; }
}
