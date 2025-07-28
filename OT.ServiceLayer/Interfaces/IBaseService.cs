using OT.ServiceLayer.DTOs;

namespace OT.ServiceLayer.Interfaces;

public interface IBaseService<TDto> where TDto : BaseDto
{
    Task<TDto?> GetByIdAsync(int id);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> CreateAsync(TDto dto);
    Task<TDto> UpdateAsync(TDto dto);
    Task DeleteAsync(int id);
}