using System.Text.RegularExpressions;
using ClozerWoods.Models.Entities;
using ClozerWoods.Models.Repositories;

namespace ClozerWoods.Services;

public class QuickTagService
{
    private IGalleryRepository _galleryRepo;
    private IMediaItemRepository _mediaItemRepo;
    private string _mediaUrl;

    public QuickTagService(IGalleryRepository galleryRepo, IMediaItemRepository mediaItemRepo, string mediaUrl) {
        _galleryRepo = galleryRepo;
        _mediaItemRepo = mediaItemRepo;
        _mediaUrl = mediaUrl;
    }

    public string? Transform(string? content) {
        if(content == null) {
            return content;
        }
        content = CompletionCheckboxes(content);
        content = Image(content);
        content = Gallery(content);
        return content;
    }

    public string CompletionCheckboxes(string content) {
        const string completeCheckbox = "<input type=\"checkbox\" checked disabled class=\"completion-checkbox\">";
        const string completeCheckboxToken = "[X]";
        const string incompleteCheckbox = "<input type=\"checkbox\" disabled class=\"completion-checkbox\">";
        const string incompleteCheckboxToken = "[]";
        return content
            .Replace(completeCheckboxToken, completeCheckbox)
            .Replace(incompleteCheckboxToken, incompleteCheckbox);
    }

    public string Image(string content) {
        /*
         * [img 0 right|left]
         */
        var regex = new Regex(@"\[img (\d+)( left| right)?\]", RegexOptions.Compiled);
        MatchCollection matches = regex.Matches(content);
        uint id;
        string position;
        GroupCollection groups;
        MediaItem? item;
        foreach(Match match in matches) {
            position = "left";
            groups = match.Groups;
            id = UInt32.Parse(groups[1].Value);
            if(groups[2].Success) {
                position = groups[2].Value;
            }
            item = _mediaItemRepo.Get(id);
            if(item == null) {
                continue;
            }
            content = content.Replace(
                groups[0].Value,
                $"<img src=\"{_mediaUrl}{Path.DirectorySeparatorChar}{item.Thumbnail}\" class=\"{position}\">"
            );
        }
        return content;
    }

    public string Gallery(string content) {
        /*
         * [gallery 0]
         */
        var regex = new Regex(@"\[gallery (\d+)\]", RegexOptions.Compiled);
        MatchCollection matches = regex.Matches(content);
        uint id;
        GroupCollection groups;
        Gallery gallery;
        foreach(Match match in matches) {
            groups = match.Groups;
            id = UInt32.Parse(groups[1].Value);
            gallery = _galleryRepo.Get(id);
            content = content.Replace(groups[0].Value,
                $"<div class=\"gallery\" data-gallery=\"{id}\"><img src=\"{_mediaUrl}{Path.DirectorySeparatorChar}{gallery.MediaItems.First().Thumbnail}\"></div>");
        }
        return content;
    }
}
