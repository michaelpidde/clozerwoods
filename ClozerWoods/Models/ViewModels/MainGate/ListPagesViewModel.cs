using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.ViewModels.MainGate;

public class ListPagesViewModel : SharedViewModel {
    public IEnumerable<Page>? PageList { get; init; }
    public bool Modified { get; init; }
}
