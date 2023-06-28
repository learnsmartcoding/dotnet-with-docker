using LearnSmartCoding.Api.ApiModels;
using LearnSmartCoding.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LearnSmartCoding.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductLogic _productLogic;        

        public ProductsController(IProductLogic productLogic)
        {
            _productLogic = productLogic;            
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts(string category = "all")
        {
            Log.Information("Starting controller action GetProducts for {category}", category);
            return _productLogic.GetProductsForCategory(category);
        }
    }
}