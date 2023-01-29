using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Utility
{
    public class Sitemap
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public object RouteValue { get; set; }
        public bool CurrentPage { get; set; }

        public static List<Sitemap> Routes
        {
            get
            {
                if (HttpContext.Current.Session["SitemapRoutes"] == null)
                    HttpContext.Current.Session["SitemapRoutes"] = new List<Sitemap>();
                return (List<Sitemap>)HttpContext.Current.Session["SitemapRoutes"];
            }
            set { HttpContext.Current.Session["SitemapRoutes"] = value; }
        }

        public static void Add(Sitemap sitemap, string currentPageTitle = null)
        {
            //remove existing current page
            var currentPage = Routes.Where(x => x.CurrentPage).FirstOrDefault();
            if (currentPage != null)
                Routes.Remove(currentPage);

            //if sitemap exists, remove all sitemaps added after that 
            var itemExist = Routes.Where(x => x.Name == sitemap.Name && x.Action == sitemap.Action && x.Controller == sitemap.Controller).FirstOrDefault();
            if(itemExist != null)
                Routes.RemoveAll(x => Routes.IndexOf(x) >= Routes.IndexOf(itemExist));

            //add sitemap
            Routes.Add(sitemap);

            //add current page, if given
            if(!string.IsNullOrEmpty(currentPageTitle))
                Routes.Add(new Sitemap() { Name = currentPageTitle, CurrentPage = true });
        }

        public static void Clear()
        {
            Routes.Clear();
        }

    }
}