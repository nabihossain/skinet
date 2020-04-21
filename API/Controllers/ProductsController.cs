using System.Collections.Generic;
using System.Threading.Tasks;
using API.SpecParams;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        public clsProduct cProducts { get; }
        public ProductsController()
        {
            cProducts = new clsProduct();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery]ProductSpecParams productParam)
        {
            var sortBy = new KeyValuePair<string, string>(productParam.Field, productParam.Sort);
            var PDList = await cProducts.GetProduct(sortBy, productParam.BrandId, productParam.TypeId,
            productParam.Page, productParam.PageSize, productParam.Search);
            return Ok(PDList);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var PDID = await cProducts.GetProductByID(id);
            return Ok(PDID);
        }
        [HttpGet("brands")]
        public async Task<IActionResult> GetProductBrands()
        {
            var PDList = await cProducts.GetProductBrand();
            return Ok(PDList);
        }
        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypes()
        {
            var PDList = await cProducts.GetProductType();
            return Ok(PDList);
        }
    }
}