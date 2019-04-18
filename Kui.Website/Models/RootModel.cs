using System.Collections.Generic;
using Kui.Core.Convert;

namespace Kui.Website.Models
{
    public class RootModel
    {
        [Text(Label="标题", MinLength=5, MaxLength=20)]
        public string Title {get; set;} = "";

        [Group(Label="横幅")]
        public BannerModel Banner {get; set;} = new BannerModel();
        public MenuModel Menu {get; set;} = new MenuModel();
        public CarouselModel Carousel {get; set;} = new CarouselModel();
        public Dictionary<string, CategoryModel> Categories {get; set;} = new Dictionary<string, CategoryModel>();
        public FooterModel Footer {get; set;} = new FooterModel();
    }
    public class BannerModel
    {
        [Text(Label="标志", MinLength=5, MaxLength=20)]
        public string Logo {get; set;} = "";

        [Text(Label="标题", MinLength=5, MaxLength=20)]
        public string Title {get; set;} = "";

        [Text(Label="副标题", MinLength=5, MaxLength=20)]
        public string Subtitle {get; set;} = "";
    }
    public class MenuModel
    {
        public List<ItemModel> MenuItems {get; set;} = new List<ItemModel>();
    }
    public class CarouselModel
    {
        public List<ItemModel> Slides {get; set;} = new List<ItemModel>();
    }
    public class FooterModel
    {
        public string Copyright {get; set;} = "";
        public string RecordNumber {get; set;} = "";
        public string Address {get; set;} = "";
    }
    
    public class CategoryModel : ItemModel
    {
        public List<ItemModel> Items {get; set;} = new List<ItemModel>();
    }
    public class ItemModel
    {
        public string Image {get; set;} = ""; 
        public string Caption {get; set;} = ""; 
        public string Href {get; set;} = ""; 
        public string Title {get; set;} = "";
        public string Content {get; set;} = "";
    }
}