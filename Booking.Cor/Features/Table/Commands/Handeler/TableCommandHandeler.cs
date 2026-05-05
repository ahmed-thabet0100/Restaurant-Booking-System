using AutoMapper;
using MediatR;
using Restaurant.Cor.Features.Table.Commands.Model;
using Restaurant.Cor.Features.Table.Queries.Result;
using Restaurant.Service;
using Restaurant.Service.Abstracts;

namespace Restaurant.Cor.Features.Table.Commands.Handeler
{
    public class TableCommandHandeler :
        IRequestHandler<AddTableCommand, Response<GetTableDto>>,
        IRequestHandler<UpdateTableCommand, Response<GetTableDto>>,
        IRequestHandler<DeleteTableCommand, Response<string>>
    {
        private readonly ITableService _service;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;

        public TableCommandHandeler(
            ITableService service,
            IMapper mapper,
            ResponseHandler responseHandler)
        {
            _service = service;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        // =========================

        public async Task<Response<GetTableDto>> Handle(AddTableCommand request, CancellationToken cancellationToken)
        {
            var table = await _service.AddTableAsync(
                _mapper.Map<Book.Data.Entities.Table>(request));

            return _responseHandler.Success(
                _mapper.Map<GetTableDto>(table),
                "Table added successfully");
        }

        // =========================

        public async Task<Response<GetTableDto>> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
        {
            var table = await _service.UpdateTableAsync(
                _mapper.Map<Book.Data.Entities.Table>(request));

            return _responseHandler.Success(
                _mapper.Map<GetTableDto>(table),
                "Table updated successfully");
        }

        // =========================

        public async Task<Response<string>> Handle(DeleteTableCommand request, CancellationToken cancellationToken)
        {
            await _service.DeleteTableAsync(request.TableNum);

            return _responseHandler.Deleted<string>();
        }
    }
}
