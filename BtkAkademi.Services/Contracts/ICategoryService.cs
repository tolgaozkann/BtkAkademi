using BtkAkademi.Entities.Dtos;

namespace BtkAkademi.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(bool trackChanges);
    Task<CategoryDto> GetOneCategoryByIdAsync(int id, bool trackChanges);
    Task<CategoryDto> CreateOneCategory(InsertCategoryDto category, bool trackChanges);
    Task UpdateOneCategoryAsync(int id, UpdateCategoryDto category, bool trackChanges);
    Task DeleteOneCategoryAsync(int id, bool trackChanges);
}