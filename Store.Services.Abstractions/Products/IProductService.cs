using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.Products;

namespace Store.Services.Abstractions.Products
{
    public interface IProductService
    {

        Task<IEnumerable<BrandResponse>> GetAllProductsAsync();

        Task<BrandResponse> GetAllProductbyIdAsync(int id);

        Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync();

        Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync();


    }
}
