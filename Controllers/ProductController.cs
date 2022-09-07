using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcClient.Controllers
{
    public class ProductController : Controller
    {
        //  public static HttpClient client = new HttpClient();
        public async Task<IActionResult> ProductDetails()

        {
            List<Product> ProductInfo = new List<Product>();
            // HttpClient client = new HttpClient();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44347/api/Products");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the product list  
                    ProductInfo = JsonConvert.DeserializeObject<List<Product>>(ProdResponse);

                }
                //returning the product list to view  
                return View(ProductInfo);
            }
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product p)
        {
            Product prodobj = new Product();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44347/api/Products", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    prodobj = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return RedirectToAction("ProductDetails");
        }
    }
}


