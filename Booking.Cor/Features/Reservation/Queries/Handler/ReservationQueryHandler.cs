using AutoMapper;
using Booking.Cor.Features.Reservation.Queries.Model;
using Booking.Cor.Features.Reservation.Queries.Result;
using Booking.Service.Abstracts;
using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Service;

namespace Booking.Cor.Features.Reservation.Queries.Handler
{
    public class ReservationQueryHandler : IRequestHandler<GetReservationForStaffQuery, Response<GetReservationForStaff>>,
                                           IRequestHandler<GetReservationForUserQuery, Response<GetReservationForUser>>,
                                           IRequestHandler<GetAllReservationForStaffQuery, Response<Pagination<GetReservationForStaff>>>,
                                           IRequestHandler<GetAllReservationForUserQuery, Response<Pagination<GetReservationForUser>>>
    {
        private readonly IBookService _service;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;

        public ReservationQueryHandler(IBookService service, IMapper mapper, ResponseHandler responseHandler)
        {
            _service = service;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<GetReservationForUser>> Handle(GetReservationForUserQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _service.GetReservation(request.ReservationId);

            if (!reservation.Succeeded)
            {
                return new Response<GetReservationForUser>
                {
                    Succeeded = false,
                    Message = reservation.Message,
                    StatusCode = reservation.StatusCode
                };
            }

            var mapped = _mapper.Map<GetReservationForUser>(reservation.Data);

            return _responseHandler.Success(mapped);
        }

        public async Task<Response<Pagination<GetReservationForStaff>>> Handle(GetAllReservationForStaffQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _service.GetAllReservationForStaff(request.SpecParams);

            if (!reservations.Succeeded)
            {
                return new Response<Pagination<GetReservationForStaff>>
                {
                    Succeeded = false,
                    Message = reservations.Message,
                    StatusCode = reservations.StatusCode
                };
            }

            var mapped = _mapper.Map<List<GetReservationForStaff>>(reservations.Data.Item1);

            var pagination = new Pagination<GetReservationForStaff>(
                request.SpecParams.PageIndex,
                request.SpecParams.PageSize,
                reservations.Data.Item2,
                mapped);

            return _responseHandler.Success(pagination);
        }

        public async Task<Response<Pagination<GetReservationForUser>>> Handle(GetAllReservationForUserQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _service.GetAllReservationForUser(request.SpecParams);

            if (!reservations.Succeeded)
            {
                return new Response<Pagination<GetReservationForUser>>
                {
                    Succeeded = false,
                    Message = reservations.Message,
                    StatusCode = reservations.StatusCode
                };
            }

            var mapped = _mapper.Map<List<GetReservationForUser>>(reservations.Data.Item1);

            var pagination = new Pagination<GetReservationForUser>(
                request.SpecParams.PageIndex,
                request.SpecParams.PageSize,
                reservations.Data.Item2,
                mapped);

            return _responseHandler.Success(pagination);
        }

        public async Task<Response<GetReservationForStaff>> Handle(GetReservationForStaffQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _service.GetReservation(request.ReservationId);

            if (!reservation.Succeeded)
            {
                return new Response<GetReservationForStaff>
                {
                    Succeeded = false,
                    Message = reservation.Message,
                    StatusCode = reservation.StatusCode
                };
            }

            var mapped = _mapper.Map<GetReservationForStaff>(reservation.Data);

            return _responseHandler.Success(mapped);
        }
    }
}
