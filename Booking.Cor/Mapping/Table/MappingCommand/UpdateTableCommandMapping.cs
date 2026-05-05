using Restaurant.Cor.Features.Table.Commands.Model;

namespace Restaurant.Cor.Mapping.Table
{
    public partial class TableProfile
    {
        public void UpdateTableCommandMapping()
        {
            CreateMap<UpdateTableCommand, Book.Data.Entities.Table>();
        }
    }
}
