using AutoMapper;
using Booking.Cor.Features.Account.Command.Model;
using MediatR;
using Restaurant.Cor.Features.Account.Command.Model;
using Restaurant.Data.Dtos;
using Restaurant.Service;
using Restaurant.Service.Abstracts;

namespace Restaurant.Cor.Features.Account.Command.Handeler
{
    public class AccountHandeler : IRequestHandler<RegisterCommand, Response<string>>,
                                   IRequestHandler<VerifyEmailCommand, Response<AuthResponseDto>>,
                                   IRequestHandler<LogInCommand, Response<AuthResponseDto>>,
                                   IRequestHandler<ForgetPasswordCommand, Response<string>>,
                                   IRequestHandler<ResetPasswordCommand, Response<string>>,
                                   IRequestHandler<ResentOtpCommand, Response<string>>,
                                   IRequestHandler<RegisterAdminCommand, Response<string>>,
                                   IRequestHandler<InviteAdminCommand, Response<string>>,
                                   IRequestHandler<ChangePasswordCommand, Response<string>>,
                                   IRequestHandler<RefreshTokenCommand, Response<AuthResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IInvetationAdminSender _invetationAdminSender;

        public AccountHandeler(IAuthService authService, IMapper mapper, IInvetationAdminSender invetationAdminSender)
        {
            _authService = authService;
            _mapper = mapper;
            _invetationAdminSender = invetationAdminSender;
        }
        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(_mapper.Map<RegisterDto>(request));
        }

        public async Task<Response<AuthResponseDto>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ConfirmOtp(_mapper.Map<VerifyEmailOtpDTo>(request));
        }

        public async Task<Response<AuthResponseDto>> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(_mapper.Map<LogInDto>(request));
        }

        public async Task<Response<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ForgetPasswordAsync(request.Email);

        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ResetPasswordAsync(_mapper.Map<ResetPasswordDto>(request));
        }

        public async Task<Response<string>> Handle(ResentOtpCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ResendOtp(_mapper.Map<ResentOtpDto>(request));

        }

        public async Task<Response<string>> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAdminAsync(_mapper.Map<RegisterAdminDto>(request));
        }

        public async Task<Response<string>> Handle(InviteAdminCommand request, CancellationToken cancellationToken)
        {
            var result = await _invetationAdminSender.InvideAdmin(request.Email);
            return result;
        }

        public async Task<Response<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ChangePasswordAsync(_mapper.Map<ChangePasswordDto>(request));
        }

        public async Task<Response<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(_mapper.Map<RefreshRequestDto>(request));
        }
    }
}
