using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Table.Queries.Result;
using Restaurant.Infra.Specifications.TableSpecifications;
using Restaurant.Service;

namespace Restaurant.Cor.Features.Table.Queries.Model
{
    public class GetAvailableTables
    : IRequest<Response<Pagination<GetTableDto>>>
    {
        public TableSpecParams SpecParams { get; set; } = new TableSpecParams();

        public int Guests { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
