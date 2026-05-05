using AutoMapper;
using Booking.Cor.Features.Reservation.Commands.Model;
using Booking.Cor.Features.Reservation.Queries.Result;
using Booking.Service.Abstracts;
using MediatR;
using Restaurant.Service;

namespace Booking.Cor.Features.Reservation.Commands.Handler
{
    public class ReservationHandler : IRequestHandler<CreateReservationCommand, Response<CreateReservationCommand>>,
                                      IRequestHandler<ApproveReservationCommand, Response<GetReservationForStaff>>,
                                      IRequestHandler<RejectReservationCommand, Response<string>>,
                                      IRequestHandler<CancelReservationCommand, Response<string>>,
                                      IRequestHandler<UpdateTimeCommand, Response<GetReservationForUser>>,
                                      IRequestHandler<CompeleteReservationCommand, Response<string>>

    {
        private readonly ResponseHandler _responseHandler;
        private readonly IBookService _service;
        private readonly IMapper _mapper;

        public ReservationHandler(ResponseHandler responseHandler, IBookService service, IMapper mapper)
        {
            _responseHandler = responseHandler;
            _service = service;
            _mapper = mapper;
        }
        public async Task<Response<CreateReservationCommand>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = _mapper.Map<Booking.Data.Entities.Reservation>(request);

            var result = await _service.CreateReservation(reservation);

            if (!result.Succeeded)
            {
                return new Response<CreateReservationCommand>
                {
                    Succeeded = false,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }

            var mapped = _mapper.Map<CreateReservationCommand>(result.Data);

            return _responseHandler.Created(mapped, result.Message);
        }

        public async Task<Response<GetReservationForStaff>> Handle(ApproveReservationCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.ApproveReservation(request.ReservationId, request.TableNumber);

            if (!result.Succeeded)
            {
                return new Response<GetReservationForStaff>
                {
                    Succeeded = false,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }

            var mapped = _mapper.Map<GetReservationForStaff>(result.Data);

            return _responseHandler.Success(mapped);
        }

        public async Task<Response<string>> Handle(RejectReservationCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.RejectReservation(request.ReservationId);

            if (!result.Succeeded)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }

            return _responseHandler.Success(result.Data, result.Message);
        }

        public async Task<Response<string>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.CancelReservation(request.ReservationId);

            if (!result.Succeeded)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }

            return _responseHandler.Success(result.Data, result.Message);
        }

        public async Task<Response<GetReservationForUser>> Handle(UpdateTimeCommand request, CancellationToken cancellationToken)
        {
            var reservation = _mapper.Map<Booking.Data.Entities.Reservation>(request);

            var result = await _service.UpdateReservationTime(reservation);

            if (!result.Succeeded)
            {
                return new Response<GetReservationForUser>
                {
                    Succeeded = false,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }

            var mapped = _mapper.Map<GetReservationForUser>(result.Data);

            return _responseHandler.Success(mapped);
        }

        public async Task<Response<string>> Handle(CompeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.CompleteReservation(request.ReservationId);

            if (!result.Succeeded)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }

            return _responseHandler.Success(result.Data, result.Message);
        }
    }
}
