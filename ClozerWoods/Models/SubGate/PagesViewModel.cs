using ClozerWoods.Models.Entities;
using ClozerWoods.Models.MainGate;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.SubGate;

public class PagesViewModel : SharedViewModel {
    public Page? SelectedPage { get; init; }
    public IEnumerable<SelectListItem>? PageListItems { get; init; }
}
