using DAL.Context;
using DAL.Entities;
using DAL.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace DAL.UnitOfWork
{
    public class UnitOfWork_Skinet
    {
        DbContext db;
        public UnitOfWork_Skinet()
        {
            db = new StoreContext();
        }
        private RepositoryBase<Product> _GetProduct;
        public RepositoryBase<Product> GetProductRepo
        {
            get
            {
                if (_GetProduct == null) _GetProduct = new RepositoryBase<Product>(db);

                return _GetProduct;
            }
        }
        private RepositoryBase<ProductType> _GetProductType;
        public RepositoryBase<ProductType> GetProductTypeRepo
        {
            get
            {
                if (_GetProductType == null) _GetProductType = new RepositoryBase<ProductType>(db);

                return _GetProductType;
            }
        }
        private RepositoryBase<ProductBrand> _GetProductBrand;
        public RepositoryBase<ProductBrand> GetProductBrandRepo
        {
            get
            {
                if (_GetProductBrand == null) _GetProductBrand = new RepositoryBase<ProductBrand>(db);

                return _GetProductBrand;
            }
        }
    }
}