using ClozerWoods.Models.Entities;
using ClozerWoods.Services;

namespace ClozerWoods.Models.ViewModels.Public;

public class PageViewModel : SharedViewModel {
    public Page SelectedPage { get; init; }
    public QuickTagService QuickTagService { get; init; }
}
