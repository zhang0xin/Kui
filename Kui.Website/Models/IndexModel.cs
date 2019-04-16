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
    public class BannerModel
    {
        public string Logo {get; set;}
        public string Title {get; set;}
        public string Subtitle {get; set;}
    }
    public class MenuModel
    {
        public List<ItemModel> MenuItems {get; set;}
    }
    public class CarouselModel
    {
        public List<ItemModel> Slides {get; set;}
    }
    public class FooterModel
    {
        public string Copyright {get; set;}
        public string RecordNumber {get; set;}
        public string Address {get; set;}
    }
    
    public class CategoryModel : ItemModel
    {
        public List<ItemModel> Items {get; set;}
    }
    public class ItemModel
    {
        public string Image {get; set;}
        public string Caption {get; set;}
        public string Href {get; set;}
        public string Title {get; set;}
        public string Content {get; set;}
    }
}