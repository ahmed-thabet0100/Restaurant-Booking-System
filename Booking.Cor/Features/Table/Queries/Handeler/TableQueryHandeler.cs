using AutoMapper;
using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Table.Queries.Model;
using Restaurant.Cor.Features.Table.Queries.Result;
using Restaurant.Service;
using Restaurant.Service.Abstracts;

namespace Restaurant.Cor.Features.Table.Queries.Handeler
{
    public class TableQueryHandeler :
        IRequestHandler<GetAllTableInRestaurant, Response<Pagination<GetTableDto>>>,
        IRequestHandler<GetTableInRestaurant, Response<GetTableDto>>,
        IRequestHandler<GetAvailableTables, Response<Pagination<GetTableDto>>>
    {
        private readonly ITableService _service;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;

        public TableQueryHandeler(
            ITableService service,
            IMapper mapper,
            ResponseHandler responseHandler)
        {
            _service = service;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        // =========================

        public async Task<Response<Pagination<GetTableDto>>> Handle(
            GetAllTableInRestaurant request,
            CancellationToken cancellationToken)
        {
            var (tables, count) =
                await _service.GetAllTablesInRestaurantAsync(request.SpecParams);

            var data = _mapper.Map<IReadOnlyList<GetTableDto>>(tables);

            var pagination = new Pagination<GetTableDto>(
                request.SpecParams.PageIndex,
                request.SpecParams.PageSize,
                count,
                data
            );

            return _responseHandler.Success(pagination, "Tables fetched successfully");
        }

        // =========================

        public async Task<Response<GetTableDto>> Handle(
            GetTableInRestaurant request,
            CancellationToken cancellationToken)
        {
            var table = await _service.GetTableByIdAsync(request.TableNum);

            return _responseHandler.Success(
                _mapper.Map<GetTableDto>(table),
                "Table fetched successfully");
        }

        public async Task<Response<Pagination<GetTableDto>>> Handle(GetAvailableTables request, CancellationToken cancellationToken)
        {
            var tables = await _service.GetAvailableTables(request.Guests, request.Start, request.End);

            var mapped = _mapper.Map<List<GetTableDto>>(tables.Data);

            var pagination = new Pagination<GetTableDto>(
                request.SpecParams.PageIndex,
                request.SpecParams.PageSize,
                tables.Count,
                mapped);

            return _responseHandler.Success(pagination);
        }
    }
}
