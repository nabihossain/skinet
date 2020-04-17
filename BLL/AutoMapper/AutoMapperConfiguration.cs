using AutoMapper;

namespace BLL.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfiles>();
                }
               );

            return config;
        }
    }
}