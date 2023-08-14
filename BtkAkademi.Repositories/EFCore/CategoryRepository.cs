using BtkAkademi.Entities.Models;
using BtkAkademi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BtkAkademi.Repositories.EFCore;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges) => await FindAll(trackChanges)
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

    public async Task<Category> GetOneCategoryByCategoryId(int categoryId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(categoryId), trackChanges)
            .SingleOrDefaultAsync();

    public void CreateOneCategory(Category category) => Create(category);

    public void DeleteOneCategory(Category category) => Delete(category);

    public void UpdateOneRepository(Category category) => Update(category);
    
}