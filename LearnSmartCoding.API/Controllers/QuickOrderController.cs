using System;
using LearnSmartCoding.Api.ApiModels;
using LearnSmartCoding.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LearnSmartCoding.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuickOrderController : ControllerBase
    {        
        private readonly IQuickOrderLogic _orderLogic;

        public QuickOrderController(IQuickOrderLogic orderLogic)
        {            
            _orderLogic = orderLogic;
        }

        [HttpPost]
        public Guid SubmitQuickOrder(QuickOrder orderInfo)
        {
            Log.Information($"Submitting order for {orderInfo.Quantity} of {orderInfo.ProductId}.");
            return _orderLogic.PlaceQuickOrder(orderInfo, 1234); // would get customer id from authN system/User claims
        }
    }
}
