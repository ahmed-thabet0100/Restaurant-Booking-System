using Restaurant.Cor.Features.Table.Commands.Model;


namespace Restaurant.Cor.Mapping.Table
{
    public partial class TableProfile
    {
        public void AddTableCommandMapping()
        {
            CreateMap<AddTableCommand, Book.Data.Entities.Table>().ReverseMap();
        }
    }
}
