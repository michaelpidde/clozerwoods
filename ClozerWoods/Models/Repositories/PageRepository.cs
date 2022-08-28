using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    public Page GetByStub(string stub) {
        Page? page = null;
        if(Pages != null) {
            page = Pages.FirstOrDefault(x => x.Stub == stub);
        }
        if(page == null) {
            throw new PageNotFoundException();
        }
        return page;
    }

    public IEnumerable<SelectListItem> GetForSelect(uint? pageId = null, string? defaultItemLabel = "* New") {
        var list = Pages
                   .OrderBy(page => page.Title)
                   .Select(p => new SelectListItem {
                       Value = p.Id.ToString(),
                       Text = p.Title,
                       Selected = p.Id == pageId,
                   });
        return list.Prepend(new SelectListItem {
            Value = "",
            Text = defaultItemLabel,
        });
    }

    public IEnumerable<Page> GetPublished =>
        Pages
        .OrderBy(x => x.Title)
        .Where(p => p.Published);

    public Page Add(Page page) {
        _context.Pages.Add(page);
        _context.SaveChanges();
        return Get(page.Title);
    }

    public Page Update(Page page) {
        _context.Entry(
            _context.Pages.FirstOrDefault(x => x.Id == page.Id)
        ).CurrentValues.SetValues(page);
        _context.SaveChanges();
        return page;
    }
}

public class PageNotFoundException : Exception { }
