﻿using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.ViewModels.MainGate;

public class MediaItemsViewModel : SharedViewModel {
    public MediaItem? SelectedMediaItem { get; init; }
    public IEnumerable<MediaItem>? MediaItemList { get; init; }
}
