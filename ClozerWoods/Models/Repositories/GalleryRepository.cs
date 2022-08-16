using ClozerWoods.Models.Entities;

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

    public Gallery Get(int id) {
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
