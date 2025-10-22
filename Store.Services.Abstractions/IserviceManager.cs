using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Services.Abstractions.Products;

namespace Store.Services.Abstractions
{
    public interface IserviceManager
    {
        IProductService ProductService { get; }
    }
}
