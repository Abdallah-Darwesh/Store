using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.Baskets;

namespace Store.Services.Abstractions.Baskets
{
    public interface IBasketService
    {
        Task<BasketDto> GetBasketAsync(string basketId);
        Task<BasketDto> CreateBasketAsync(BasketDto basket, TimeSpan duration);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
