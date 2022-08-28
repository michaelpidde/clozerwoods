using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.Repositories;

public interface IMediaItemRepository {
    IEnumerable<MediaItem> MediaItems { get; }
    MediaItem Get(uint id);
    IEnumerable<SelectListItem> GetForSelect(uint? mediaItemId = null);
    MediaItem Add(MediaItem mediaItem);
    MediaItem Update(MediaItem mediaItem);
}
