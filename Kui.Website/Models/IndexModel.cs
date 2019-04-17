using System.Collections.Generic;

namespace Kui.Website.Models
{
    public class IndexModel
    {
        public string Title {get; set;}
        public BannerModel Banner {get; set;}
        public MenuModel Menu {get; set;}
        public CarouselModel Carousel {get; set;}
        public Dictionary<string, CategoryModel> Categories {get; set;}
        public FooterModel Footer {get; set;}
    }
}