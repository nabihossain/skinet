using System.Configuration;
using AutoMapper;
using BLL.Dtos;
using DAL.Entities;

namespace BLL.AutoMapper
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                string UrlString = ConfigurationManager.AppSettings["ApiUrl"];
                return UrlString + source.PictureUrl;
            }

            return null;
        }
    }
}