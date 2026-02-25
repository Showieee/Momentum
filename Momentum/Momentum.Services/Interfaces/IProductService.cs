using Momentum.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momentum.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductModel>> Get();

        Task<ProductModel?> GetById(Guid id);

        Task Insert(ProductModel product);

        Task Update(Guid id, ProductModel product);
        Task DeleteById(Guid id);
        Task Delete(Guid id);
    }
}
