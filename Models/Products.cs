using System.ComponentModel.DataAnnotations;

namespace WebScrappersApplication.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        public string?  url { get; set; }
        public string? Price { get; set; }

        public string? imgUrl { get; set; }
      

    }
}
