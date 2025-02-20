using System.ComponentModel.DataAnnotations;
using AlloyDemoKit.Models.Media;
using EPiServer.Core;
using EPiServer.Web;

namespace AlloyDemoKit.Models.Pages
{
    /// <summary>
    /// Used primarily for publishing news articles on the website
    /// </summary>
    [SiteContentType(
        GroupName = Global.GroupNames.News,
        GUID = "AEECADF2-3E89-4117-ADEB-F8D43565D2F4")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    public class ArticlePage : StandardPage
    {
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }
    }
}
