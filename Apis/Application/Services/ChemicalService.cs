using Application.Commons.Exceptions;
using Application.Commons.Models;
using Application.DTO.Chemicals;
using Application.Interfaces;
using Application.Interfaces.Chemicals;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class ChemicalService : IChemicalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserService _currentUserService;

    public ChemicalService(
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

    public Task<IReadOnlyCollection<ChemicalDTO>> GetAllAsync(
        CancellationToken cancellationToken = default)
    => _unitOfWork.ChemicalRepository.GetAllAsync<ChemicalDTO>();

    public Task<PagedList<ChemicalDTO>> GetPagedListAsync(
        GetPagedChemicalsQuery request,
        CancellationToken cancellationToken = default)
    => _unitOfWork.ChemicalRepository.GetPageListAsync<ChemicalDTO>(
        orderBy: x => x.OrderBy(x => x.CreatedAt),
        pageNumber: request.PageNumber,
        pageSize: request.PageSize);

    public async Task<ChemicalDTO> GetOneAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.ChemicalRepository.SingleOrDefaultAsync<ChemicalDTO>(
            where: x => x.Id == id,
            cancellationToken: cancellationToken);

        if (item is null)
            throw new NotFoundException(nameof(Chemical), id);

        return item;
    }

    public async Task<ChemicalDTO> CreateAsync(
        ChemicalCreateDTO dto,
        CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Chemical>(dto);

        // TODO: move to event handler
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = _currentUserService.UserId;

        await _unitOfWork.ChemicalRepository.AddAsync(entity);
        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveResult is not > 0)
            throw new TransactionException("Create chemical failed");
        return _mapper.Map<ChemicalDTO>(entity);
    }

    public async Task<ChemicalDTO> UpdateAsync(
        UpdateChemicalCommand request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChemicalRepository.SingleOrDefaultAsync(
            where: x => x.Id == request.Id,
            tracked: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Chemical), request.Id);

        _mapper.Map(request, entity);

        // TODO: move to event handler
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = _currentUserService.UserId;

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveResult is not > 0)
            throw new TransactionException("Update chemical failed");
        var result = _mapper.Map<ChemicalDTO>(entity);
        return result;
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ChemicalRepository.SingleOrDefaultAsync(
            where: x => x.Id == id,
            tracked: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Chemical), id);

        // TODO: move to event handler
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = _currentUserService.UserId;

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveResult is not > 0)
            throw new TransactionException("Update chemical failed");
    }
}
