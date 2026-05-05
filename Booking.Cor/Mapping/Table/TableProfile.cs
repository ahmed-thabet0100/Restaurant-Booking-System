using AutoMapper;

namespace Restaurant.Cor.Mapping.Table
{
    public partial class TableProfile : Profile
    {
        public TableProfile()
        {
            //commands
            AddTableCommandMapping();
            UpdateTableCommandMapping();

            //Queries
            GetTableDtoMappingQuery();
        }
    }
}
