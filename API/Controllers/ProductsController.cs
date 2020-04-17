using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ProductsController : ControllerBase
    {
        public clsProduct cProducts { get; }
        public ProductsController()
        {
            cProducts = new clsProduct();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var PDList = await cProducts.GetProduct();
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