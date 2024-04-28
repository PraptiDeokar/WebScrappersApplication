using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebScrappersApplication.Models;
using WebScrappersApplication;
using System.Diagnostics.Eventing.Reader;
using WebScrappersApplication.Data;

namespace WebScrappersApplication.Controllers
{
    public class HomeController : Controller
    {
         private readonly ILogger<HomeController> _logger;
        private readonly AppdbContext db;
        public HomeController(ILogger<HomeController> logger,AppdbContext _db)
        {
            _logger = logger;
            db= _db;
        }
        public static List<string> urllist;

        public IActionResult MainPage()
        {
            return View();
        }
        public IActionResult Index(IFormCollection key)
        {
            List<string> urls = ["https://www.reliancedigital.in/search?q=:relevance:productTags:affordable-mobiles" ,
             "https://www.shopclues.com/smartphone-sales.html",
            "https://www.vijaysales.com/mobiles-and-tablets/brand",
            "https://www.croma.com/phones-wearables/c/1",
            "https://www.sahivalue.com/categories/buy-refurbished-second-hand-samsung-galaxy-mobile-phone-fold/293890000027150104"


            ];
               //take keyword from user

          var keyword= key["key"];
           

            foreach (string url in urls) {

                HttpClient client = new HttpClient();
                var res = client.GetStringAsync(url).Result;

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(res);


                urllist = TraverseHtmlNodes(htmlDocument.DocumentNode, keyword, url);
               

            }


           List<Products> productsList2= GetDetais(urllist, keyword);

            return View(productsList2);
        }

        public List<Products> GetDetais(List<string> urllinks, string keyword)
        {
            List<Products> Productdetails = new List<Products>();

            foreach (var link in urllinks)
            {
                HttpClient client = new HttpClient();

                var res = client.GetStringAsync(link).Result;

                HtmlDocument htmlDocument2 = new HtmlDocument();
                htmlDocument2.LoadHtml(res);
              
                Productdetails= TraverseNodes(htmlDocument2.DocumentNode, link, keyword);

            }
            return Productdetails;
        }

        public List<string> urllinks = new List<string>();
        public List<string> TraverseHtmlNodes(HtmlNode node, string keyword, string url)
        {


            if (node.InnerHtml.Contains(keyword))
            {
                var baseUri = new Uri(url);

                foreach (var childNode in node.ChildNodes)
                {
                    if (childNode.Attributes.Contains("href"))
                    {

                        var links = childNode.Attributes["href"].Value;

                        if (links.Contains(keyword.ToLower()))
                        {

                            urllinks.Add(new Uri(baseUri, links).AbsoluteUri);

                        }
                    }
                    TraverseHtmlNodes(childNode, keyword, url);
                }

            }
            return urllinks;
        }
        public  List<Products> productsList = new List<Products>();

        public List<Products>  TraverseNodes(HtmlNode node, string url, string keyword)
        {
            Products p = new Products();

            //Heading
            var headerTags = new string[] { "h1", "h2", "h3", "p" };
            // var title=node.SelectSingleNode("//h1").InnerText;
            var h1nodes = node.SelectSingleNode("//h1");
            var h2nodes = node.SelectSingleNode("//h2");
            var pnodes = node.SelectSingleNode("//span");

            if (h1nodes != null)
            {
                // var header = node.SelectSingleNode("//h1").InnerHtml.Contains(keyword);
                //Console.WriteLine(h1nodes.InnerText);
                //Console.WriteLine(url);
                p.Name = h1nodes.InnerText.Trim();
                p.url = url.Trim();
                var parent = h1nodes.ParentNode;
                var fullNode = parent.SelectNodes("//span");
                foreach (var sNode in fullNode)
                {
                    if (sNode.InnerText.Contains("$"))
                    {
                        p.Price = sNode.InnerText.Replace("$", "");
                        break;
                    }
                    else if (sNode.InnerText.Contains("₹"))
                    {
                        p.Price = sNode.InnerText.Replace("₹", "");
                        break;
                    }
                    else if (sNode.InnerText.Contains("Rs"))
                    {
                        p.Price = sNode.InnerText.Replace("Rs", "");
                        break;
                    }
                }
            }

            else if (h2nodes != null)
            {
                p.Name = h2nodes.InnerText;
                var parent = h2nodes.ParentNode;
                var fullNode = parent.SelectNodes("//span");
                foreach (var sNode in fullNode)
                {
                    if (sNode.InnerText.Contains("$"))
                    {
                        p.Price = sNode.InnerText.Replace("$", "");
                        break;
                    }
                    else if (sNode.InnerText.Contains("₹"))
                    {
                        p.Price = sNode.InnerText.Replace("₹", "");
                        break;
                    }
                    else if (sNode.InnerText.Contains("Rs"))
                    {
                        p.Price = sNode.InnerText.Replace("Rs", "");
                        break;
                    }
                }
            }
            else if (pnodes != null)
            {
                p.Name = pnodes.InnerText;
                var parent = pnodes.ParentNode;
                var fullNode = parent.SelectNodes("//span");
                foreach (var sNode in fullNode)
                {
                    if (sNode.InnerText.Contains("$"))
                    {
                        p.Price = sNode.InnerText.Replace("$", "");
                        break;
                    }
                    else if (sNode.InnerText.Contains("₹"))
                    {
                        p.Price = sNode.InnerText.Replace("₹", "");
                        break;
                    }
                    else if (sNode.InnerText.Contains("Rs"))
                    {
                        p.Price = sNode.InnerText.Replace("Rs", "");
                        break;
                    }
                }

            }

            productsList.Add(p);
            db.Add(p);
            db.SaveChanges();
            return productsList;
        }
    }
}

