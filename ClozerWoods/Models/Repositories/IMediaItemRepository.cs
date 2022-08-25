using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.Repositories;

public interface IMediaItemRepository {
    IEnumerable<MediaItem> MediaItems { get; }
    MediaItem Get(uint id);
    MediaItem Add(MediaItem mediaItem);
    MediaItem Update(MediaItem mediaItem);
}
