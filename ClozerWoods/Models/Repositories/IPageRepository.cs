using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.Repositories;

public interface IPageRepository {
    IEnumerable<Page> Pages { get; }
    Page Get(uint id);
    Page Get(string title);
    IEnumerable<SelectListItem> GetForSelect(uint? pageId = null, string? defaultItemLabel = "* New");
    Page Add(Page page);
    Page Update(Page page);
}
