using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.Repositories;

public interface IPageRepository {
    IEnumerable<Page> Pages { get; }
    Page Get(int id);
    Page Get(string title);
    Page Add(Page page);
    Page Update(Page page);
}
