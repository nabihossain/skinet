using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public class clsProduct
    {
        UnitOfWork_Skinet UnitOfWork;
        DbContext db;
        public clsProduct()
        {
            UnitOfWork = new UnitOfWork_Skinet();
            db = new StoreContext();
        }
        public async Task<IEnumerable<Product>> GetProduct()
        {
            try
            {
                var PType = await UnitOfWork.GetProductRepo.Get(null, null, null, null, u => u.ProductBrand, u => u.ProductType);
                return PType;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
            }
        }
        public async Task<IEnumerable<Product>> GetProductByID(int ID)
        {
            try
            {
                var PType = await UnitOfWork.GetProductRepo.Get(filter: u => u.Id == ID, null, null, null, u => u.ProductBrand, u => u.ProductType);
                return PType;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
            }
        }
        public async Task<IEnumerable<ProductType>> GetProductType()
        {
            try
            {
                var PType = await UnitOfWork.GetProductTypeRepo.Get();
                return PType;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
            }
        }
        public async Task<IEnumerable<ProductBrand>> GetProductBrand()
        {
            try
            {
                var PType = await UnitOfWork.GetProductBrandRepo.Get();
                return PType;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
            }
        }
    }
}