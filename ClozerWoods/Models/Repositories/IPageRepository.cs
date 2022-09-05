using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.Repositories;

public interface IPageRepository {
    IEnumerable<Page> Pages { get; }
    Page Get(uint id);
    Page Get(string title);
    Page GetByStub(string stub);
    IEnumerable<SelectListItem> GetForSelect(uint? pageId = null, string? defaultItemLabel = "* New");
    IEnumerable<Page> GetPublished(bool excludeHome = false, bool excludeChildren = false);
    Page GetHome { get; }
    Page Add(Page page);
    Page Update(Page page);
}
