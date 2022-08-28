using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.Repositories;

public class GalleryRepository : IGalleryRepository {
    private readonly ApplicationDbContext _context;

    public GalleryRepository(ApplicationDbContext context) {
        _context = context;
    }
    public IEnumerable<Gallery> Galleries => _context.Galleries;

    public Gallery Add(Gallery gallery) {
        _context.Galleries.Add(gallery);
        _context.SaveChanges();
        return Get(gallery.Title);
    }

    public Gallery Get(uint id) {
        Gallery? gallery = null;
        if(Galleries != null) {
            gallery = Galleries.FirstOrDefault(x => x.Id == id);
        }
        if(gallery == null) {
            throw new GalleryNotFoundException();
        }
        return gallery;
    }

    public Gallery Get(string title) {
        Gallery? gallery = null;
        if(Galleries != null) {
            gallery = Galleries.FirstOrDefault(x => x.Title == title);
        }
        if(gallery == null) {
            throw new GalleryNotFoundException();
        }
        return gallery;
    }

    public IEnumerable<SelectListItem> GetForSelect(uint? galleryId = null, string? defaultItemLabel = "* New") {
        var list = Galleries
                   .OrderBy(gallery => gallery.Title)
                   .Select(g => new SelectListItem {
                       Value = g.Id.ToString(),
                       Text = g.Title,
                       Selected = g.Id == galleryId,
                   });
        return list.Prepend(new SelectListItem {
            Value = "",
            Text = defaultItemLabel,
        });
    }

    public Gallery Update(Gallery gallery) {
        Gallery? toUpdate = null;
        try {
            toUpdate = Get(gallery.Id);
        } catch(GalleryNotFoundException) {
            // TODO: Log this or something. This really should not happen.
        }
        if(toUpdate != null) {
            toUpdate.Title = gallery.Title;
            _context.SaveChanges();
        }
        return toUpdate!;
    }
}

public class GalleryNotFoundException : Exception { }
