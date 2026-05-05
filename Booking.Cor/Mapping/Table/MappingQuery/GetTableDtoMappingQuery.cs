using Restaurant.Cor.Features.Table.Queries.Result;

namespace Restaurant.Cor.Mapping.Table
{
    public partial class TableProfile
    {
        public void GetTableDtoMappingQuery()
        {
            CreateMap<Book.Data.Entities.Table, GetTableDto>().ReverseMap();
        }

    }
}
