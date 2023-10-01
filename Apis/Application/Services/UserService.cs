using Application.Commons.Exceptions;
using Application.Commons.Models;
using Application.DTO.Users;
using Application.Interfaces;
using Application.Interfaces.Users;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    public Task<IReadOnlyCollection<UserDTO>> GetAllAsync(
        CancellationToken cancellationToken = default)
    => _unitOfWork.UserRepository.GetAllAsync<UserDTO>();

    public Task<PagedList<UserDTO>> GetPagedListAsync(
        GetPagedUsersQuery request,
        CancellationToken cancellationToken = default)
    => _unitOfWork.UserRepository.GetPageListAsync<UserDTO>(
        orderBy: x => x.OrderBy(x => x.CreatedAt),
        pageNumber: request.PageNumber,
        pageSize: request.PageSize);

    public async Task<UserDTO> GetOneAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.UserRepository.SingleOrDefaultAsync<UserDTO>(
            where: x => x.Id == id,
            cancellationToken: cancellationToken);

        if (item is null)
            throw new NotFoundException(nameof(User), id);

        return item;
    }

    public async Task<UserDTO> UpdateAsync(
        UpdateUserCommand request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
            where: x => x.Id == request.Id,
            tracked: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(User), request.Id);

        _mapper.Map(request, entity);

        // TODO: move to event handler
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = _currentUserService.UserId;

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveResult is not > 0)
            throw new TransactionException("Update User failed");
        var result = _mapper.Map<UserDTO>(entity);
        return result;
    }
}
