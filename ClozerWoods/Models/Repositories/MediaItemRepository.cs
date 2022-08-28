using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.Repositories;

public class MediaItemRepository : IMediaItemRepository {
    private readonly ApplicationDbContext _context;

    public MediaItemRepository(ApplicationDbContext context) {
        _context = context;
    }

    public IEnumerable<MediaItem> MediaItems => _context.MediaItems;

    public MediaItem Add(MediaItem mediaItem) {
        _context.MediaItems.Add(mediaItem);
        _context.SaveChanges();
        return Get(mediaItem.Id);
    }

    public MediaItem Get(uint id) {
        MediaItem? mediaItem = null;
        if(MediaItems != null) {
            mediaItem = MediaItems.FirstOrDefault(x => x.Id == id);
        }
        if(mediaItem == null) {
            throw new MediaItemNotFoundException();
        }
        return mediaItem;
    }

    public IEnumerable<SelectListItem> GetForSelect(uint? mediaItemId = null) {
        var list = MediaItems
                   .OrderBy(m => m.Title)
                   .Select(m => new SelectListItem {
                       Value = m.Id.ToString(),
                       Text = m.Title,
                       Selected = m.Id == mediaItemId,
                   });
        return list.Prepend(new SelectListItem {
            Value = "",
            Text = "* New",
        });
    }

    public MediaItem Update(MediaItem mediaItem) {
        _context.Entry(
            _context.MediaItems.FirstOrDefault(x => x.Id == mediaItem.Id)
        ).CurrentValues.SetValues(mediaItem);
        _context.SaveChanges();
        return mediaItem;
    }
}

public class MediaItemNotFoundException : Exception { }
