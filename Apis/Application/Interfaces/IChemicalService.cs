using Application.Commons.Models;
using Application.DTO.Chemicals;
using Application.Interfaces.Chemicals;

namespace Application.Interfaces;

public interface IChemicalService
{
    Task<IReadOnlyCollection<ChemicalDTO>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<PagedList<ChemicalDTO>> GetPagedListAsync(
        GetPagedChemicalsQuery request,
        CancellationToken cancellationToken = default);
    Task<ChemicalDTO> GetOneAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task<ChemicalDTO> CreateAsync(
        ChemicalCreateDTO dto,
        CancellationToken cancellationToken = default);
    Task<ChemicalDTO> UpdateAsync(
        UpdateChemicalCommand request,
        CancellationToken cancellationToken = default);
    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
