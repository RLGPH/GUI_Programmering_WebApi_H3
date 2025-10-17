using Mapster;
using GUI_Programmering_WebApi.Models;

namespace GUI_Programmering_WebApi
{
    public static class MapsterConfiguration
    {
        public static void ConfigureMappings()
        {
            // Products
            TypeAdapterConfig<Product, ProductDTO>.NewConfig();
            TypeAdapterConfig<Product, ProductWithIdDTO>.NewConfig();

            TypeAdapterConfig<Product, ProductWithRelationDTO>
                .NewConfig()
                .Map(dest => dest.Category, src => src.Category.Adapt<CategoryDTO>());

            TypeAdapterConfig<Product, ProductWithImageDTO>
                .NewConfig()
                .Map(dest => dest.Category, src => src.Category.Adapt<CategoryDTO>())
                .Map(dest => dest.Image, src => src.Image.Adapt<ImageDTO>());

            // Categories
            TypeAdapterConfig<Category, CategoryDTO>.NewConfig();
            TypeAdapterConfig<Category, CategoryWithIdDTO>.NewConfig();

            // Images
            TypeAdapterConfig<Image, ImageDTO>
                .NewConfig()
                .Map(dest => dest.ImageUrl, src => src.ImageUrl);
        }
    }
}
