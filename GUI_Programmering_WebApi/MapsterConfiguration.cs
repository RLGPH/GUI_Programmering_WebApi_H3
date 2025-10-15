using Mapster;
using GUI_Programmering_WebApi.Models;

namespace GUI_Programmering_WebApi
{
    public static class MapsterConfiguration
    {
        public static void ConfigureMappings()
        {
            TypeAdapterConfig<Product, ProductDTO>.NewConfig();
            TypeAdapterConfig<Product, ProductWithIdDTO>.NewConfig();
            TypeAdapterConfig<Category, CategoryDTO>.NewConfig();
            TypeAdapterConfig<Category, CategoryWithIdDTO>.NewConfig();
            TypeAdapterConfig<Product, ProductWithRelationDTO>
                .NewConfig()
                .Map(dest => dest.Category, src => src.Category.Adapt<CategoryDTO>());
        }
    }
}
