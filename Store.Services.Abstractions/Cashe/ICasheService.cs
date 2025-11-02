using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Cashe
{
    public interface ICasheService
    {
       Task SetAsync(string key, object value, TimeSpan? expiration = null); 
       Task<String> GetAsync(string key, object value); 
    }
}
