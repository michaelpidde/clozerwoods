using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.ViewModels.SubGate;

public class PageViewModel : SharedViewModel {
    public Page? SelectedPage { get; init; }
    public IEnumerable<SelectListItem>? PageListItems { get; init; }
}
