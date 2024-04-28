using HtmlAgilityPack;
using WebScrappersApplication.Data;
using WebScrappersApplication.Models;

namespace WebScrappersApplication
{
    
    public class ScraperClass
    {
        private readonly AppdbContext db;
        public ScraperClass(AppdbContext _db)
        {
            db = _db;
        }
        public ScraperClass()
        {

        }
        ScraperClass s ;
        public  void GetDetais(List<string> urllinks, string keyword)
        {
           
           
            foreach (var link in urllinks)
            {
                HttpClient client = new HttpClient();

                var res = client.GetStringAsync(link).Result;

                HtmlDocument htmlDocument2 = new HtmlDocument();
                htmlDocument2.LoadHtml(res);
                s.TraverseNodes(htmlDocument2.DocumentNode, link, keyword);

            }
        }

      public    List<string> urllinks = new List<string>();
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
       public static  List<Products> productsList = new List<Products>();

       public void TraverseNodes(HtmlNode node, string url, string keyword)
        {
            Products p=new Products();

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
                p.Name = h1nodes.InnerText;
                p.url = url;
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
                p.Name=pnodes.InnerText;
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
        }
    }
}   
