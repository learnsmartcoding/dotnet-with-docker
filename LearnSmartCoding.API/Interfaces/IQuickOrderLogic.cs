using System;
using LearnSmartCoding.Api.ApiModels;

namespace LearnSmartCoding.Api.Interfaces
{
    public interface IQuickOrderLogic
    {
        Guid PlaceQuickOrder(QuickOrder order, int customerId);
    }
}
