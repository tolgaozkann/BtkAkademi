using BtkAkademi.Entities.Models;

namespace BtkAkademi.Repositories.Contracts;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
    Task<Category> GetOneCategoryByCategoryId(int categoryId, bool trackChanges);
    void CreateOneCategory(Category category);
    void DeleteOneCategory(Category category);
    void UpdateOneRepository(Category category);
}