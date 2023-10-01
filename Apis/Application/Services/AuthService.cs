using Application.Commons.Exceptions;
using Application.Commons.Extensions;
using Application.Commons.Models;
using Application.DTO.Users;
using Application.Interfaces;
using Application.Interfaces.Auths;
using AutoMapper;
using Domain.Entities;
using FluentValidation.Results;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService,
        IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
        _jwtService = jwtService;
    }

    public async Task<GetCurrentUserIdResponse> GetCurrentUserIdAsync()
    {
        var getUserIdTask = Task.Run(() => _currentUserService.UserId);
        return new GetCurrentUserIdResponse()
        {
            Id = await getUserIdTask
        };
    }

    public async Task<UserDTO> GetCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
        {
            throw new NotFoundException(nameof(User));
        }
        var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync<UserDTO>(
            where: x => x.Id == userId,
            cancellationToken: cancellationToken);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), userId);
        };
        return user;
    }

    public async Task RegisterAsync(
        RegisterCommand request,
        CancellationToken cancellationToken = default)
    {
        var isExisted = await _unitOfWork.UserRepository.AnyAsync(x => x.Username == request.Username, cancellationToken);
        if (isExisted)
            throw new ValidationException(new List<ValidationFailure>(){
                new ValidationFailure("Register", "Username already exists."),
            });

        var entity = _mapper.Map<User>(request);

        // TODO: move to event handler
        entity.CreatedBy = _currentUserService.UserId;

        _unitOfWork.UserRepository.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<LoginResponse> LoginAsync(
        LoginQuery request,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
            where: x => x.Username == request.Username,
            cancellationToken: cancellationToken);
        if (user is null)
        {
            throw new ValidationException(new List<ValidationFailure>(){
                new ValidationFailure("Login", "Login failed, please check your credentials. (bật mí luôn là sai tên đăng nhập rồi)"),
            });
        };
        bool isValid = request.Password.Verify(user.PasswordHash);
        if (!isValid)
        {
            throw new ValidationException(new List<ValidationFailure>(){
                new ValidationFailure("Login", "Login failed, please check your credentials. (bật mí luôn là sai mật khẩu rồi)"),
            });
        };
        //if user was found generate JWT Token
        var currentUser = new CurrentUser()
        {
            Id = user.Id,
            Username = user.Username,
            DateOfBirth = user.DateOfBirth,
            Role = user.Role,
        };
        var token = _jwtService.GenerateJWT(currentUser);

        var result = new LoginResponse()
        {
            Token = token,
            CurrentUser = currentUser,
        };
        return result;
    }
}
