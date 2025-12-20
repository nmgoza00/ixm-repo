
using IXM.DB;
using static IXM.DB.QueryRepository;

namespace IXM.API.Services
{
    public interface IStoreService
    {



        string GetProducts(string pProductId);
        string GetProductCategory(string pProductCategoryId);


    }
}
