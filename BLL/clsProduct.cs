using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BLL.AutoMapper;
using BLL.Dtos;
using DAL.Entities;
using DAL.Specifications;
using DAL.UnitOfWork;
namespace BLL
{
    public class clsProduct
    {
        UnitOfWork_Skinet UnitOfWork;
        private readonly IMapper _mapper;

        public clsProduct()
        {
            var config = new AutoMapperConfiguration().Configure();
            _mapper = config.CreateMapper();
            UnitOfWork = new UnitOfWork_Skinet();
        }
        public async Task<Pagination<ProductToReturnDto>> GetProduct(KeyValuePair<string, string> sort, int? brandId, int? typeId, int? page, int? pageSize, string search)
        {
            try
            {
                Func<IQueryable<Product>, IOrderedQueryable<Product>> order = null;
                Expression<Func<Product, bool>> filter = null;
                filter = c =>
                ((string.IsNullOrWhiteSpace(search)) || c.Name.ToLower().Contains(search)) &&
                  (!brandId.HasValue || c.ProductBrandId == brandId) &&
                  (!typeId.HasValue || c.ProductTypeId == typeId);

                if (sort.Key != null)
                {
                    order = QueryExtensions.GetOrderByFunc<Product>(sort);
                }
                var PDList = await UnitOfWork.GetProductRepo.Get(filter,
                order,
                // orderBy: o => o.OrderByDescending(d => d.ProductType),
                page, pageSize, u => u.ProductBrand, u => u.ProductType);
                var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(PDList);
                var totalItems = await UnitOfWork.GetProductRepo.CountAsync(filter, u => u.ProductBrand, u => u.ProductType);
                var returnList = new Pagination<ProductToReturnDto>(page, pageSize, totalItems, data);
                return returnList;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
            }
        }

        public async Task<IEnumerable<ProductToReturnDto>> GetProductByID(int ID)
        {
            Expression<Func<Product, bool>> filter = null;
            filter = c => c.Id == ID;
            try
            {
                var PDListByID = await UnitOfWork.GetProductRepo.Get(filter, null, null, null, u => u.ProductBrand, u => u.ProductType);
                IEnumerable<ProductToReturnDto> returnlist = _mapper.Map<List<ProductToReturnDto>>(PDListByID);
                return returnlist;
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