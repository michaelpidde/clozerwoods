using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.Repositories;

public class PageRepository : IPageRepository {
    private readonly ApplicationDbContext _context;

    public PageRepository(ApplicationDbContext context) {
        _context = context;
    }

    public IEnumerable<Page> Pages => _context.Pages;

    public Page Get(uint id) {
        Page? page = null;
        if(Pages != null) {
            page = Pages.FirstOrDefault(x => x.Id == id);
        }
        if(page == null) {
            throw new PageNotFoundException();
        }
        return page;
    }

    public Page Get(string title) {
        Page? page = null;
        if(Pages != null) {
            page = Pages.FirstOrDefault(x => x.Title == title);
        }
        if(page == null) {
            throw new PageNotFoundException();
        }
        return page;
    }

    public Page Add(Page page) {
        _context.Pages.Add(page);
        _context.SaveChanges();
        return Get(page.Title);
    }

    public Page Update(Page page) {
        Page? toUpdate = null;
        try {
            toUpdate = Get(page.Id);
        } catch(PageNotFoundException) {
            // TODO: Log this or something. This really should not happen.
        }
        if(toUpdate != null) {
            toUpdate.Title = page.Title;
            toUpdate.Content = page.Content;
            toUpdate.ParentId = page.ParentId;
            toUpdate.Published = page.Published;
            _context.SaveChanges();
        }
        return toUpdate!;
    }
}

public class PageNotFoundException : Exception { }
