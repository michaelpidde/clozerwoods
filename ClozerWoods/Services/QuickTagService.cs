namespace ClozerWoods.Services;

public class QuickTagService
{
    public static string? Transform(string? content) {
        if(content == null) {
            return content;
        }
        const string completeCheckbox = "<input type=\"checkbox\" checked disabled class=\"completion-checkbox\">";
        const string completeCheckboxToken = "[X]";
        const string incompleteCheckbox = "<input type=\"checkbox\" disabled class=\"completion-checkbox\">";
        const string incompleteCheckboxToken = "[]";
        return content
            .Replace(completeCheckboxToken, completeCheckbox)
            .Replace(incompleteCheckboxToken, incompleteCheckbox);
    }
}
