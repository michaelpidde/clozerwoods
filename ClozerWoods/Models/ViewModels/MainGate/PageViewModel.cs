using ClozerWoods.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClozerWoods.Models.ViewModels.MainGate;

public class PageViewModel : SharedViewModel {
    public Page? SelectedPage { get; init; }
    public IEnumerable<SelectListItem>? PageList { get; init; }
    public IEnumerable<SelectListItem>? ParentPageList { get; init; }
}
