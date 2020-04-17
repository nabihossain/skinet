using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.AutoMapper;
using BLL.Dtos;
using DAL.Entities;
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
        public async Task<IEnumerable<ProductToReturnDto>> GetProduct()
        {
            try
            {
                var PDList = await UnitOfWork.GetProductRepo.Get(null, null, null, null, u => u.ProductBrand, u => u.ProductType);
                IEnumerable<ProductToReturnDto> returnlist = _mapper.Map<List<ProductToReturnDto>>(PDList);
                return returnlist;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
            }
        }
        public async Task<IEnumerable<ProductToReturnDto>> GetProductByID(int ID)
        {
            try
            {
                var PDListByID = await UnitOfWork.GetProductRepo.Get(filter: u => u.Id == ID, null, null, null, u => u.ProductBrand, u => u.ProductType);
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