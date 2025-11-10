using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Services.Abstractions.Auth;
using Store.Services.Abstractions.Baskets;
using Store.Services.Abstractions.Cashe;
using Store.Services.Abstractions.Orders;
using Store.Services.Abstractions.Products;

namespace Store.Services.Abstractions
{
    public interface IserviceManager
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get; }
        ICasheService CasheService { get; }

        IAuthService AuthService { get; }
         IOrderService OrderService { get; }

    }
}
