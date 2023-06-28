using System.Collections.Generic;
using LearnSmartCoding.Api.ApiModels;

namespace LearnSmartCoding.Api.Interfaces
{
    public interface IProductLogic
    {
        IEnumerable<Product> GetProductsForCategory(string category);
    }
}
