using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace AlloyDemoKit.Models.Pages
{
    [ContentType(GroupName = Global.GroupNames.News,
        DisplayName = "ShowcasePage", 
        GUID = "4b1d2a54-3f1a-4958-a7ff-78fcd64e543b")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    public class ShowcasePage : StandardPage
    {
        [UIHint(UIHint.Image)]
        public virtual ContentReference ImageHuge { get; set; }

        [UIHint(UIHint.Image)]
        public virtual ContentReference ImageSmallWidth { get; set; }
        
        [UIHint(UIHint.Image)]
        public virtual ContentReference ImageSmallHeight { get; set; }
        
        [UIHint(UIHint.Image)]
        public virtual ContentReference ImageSmallBoth { get; set; }
        
    }
}