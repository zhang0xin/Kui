using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kui.Website.Controllers 
{
    public class FrontController : Controller
    {
        [Route("/Front/Index/{modelStyle}/{viewStyle}")]
        public IActionResult Index(string modelStyle, string viewStyle)
        {
            var model = new IndexModel(){
                Title = "标题",
                Banner = new BannerModel(){
                    Title = "创元方大电器",
                    Subtitle = "CHUANGYUAN Fangda Electric",
                    Logo = "https://bulma.io/images/bulma-logo.png"
                },
                Menu = new MenuModel(){
                     MenuItems = new List<ItemModel>(){
                         new ItemModel{ Caption = "网站首页", Href="/Front"},
                         new ItemModel{ Caption = "成套设备", Href="/Front"},
                         new ItemModel{ Caption = "电线电缆", Href="/Front"},
                         new ItemModel{ Caption = "电器元件", Href="/Front"},
                         new ItemModel{ Caption = "自动化仪表", Href="/Front"},
                         new ItemModel{ Caption = "成功案例", Href="/Front"},
                         new ItemModel{ Caption = "新闻中心", Href="/Front"},
                         new ItemModel{ Caption = "关于我们", Href="/Front"}
                     }
                }
            }; 
            return View($"{modelStyle}-{viewStyle}-index", model);
        }
    }
}