using AutoMapper;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Exceptions;
using BtkAkademi.Entities.Models;
using BtkAkademi.Repositories.Contracts;
using BtkAkademi.Services.Contracts;

namespace BtkAkademi.Services;

public class CategoryManager : ICategoryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CategoryManager(IRepositoryManager repositoryManager, IMapper mapper, ILoggerService logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(bool trackChanges)
    {
        var categories = await _repositoryManager.Category
            .GetAllCategoriesAsync(trackChanges);

        return  _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
        
    

    public async Task<CategoryDto> GetOneCategoryByIdAsync(int id, bool trackChanges)
    {
        var entity = await _repositoryManager.Category.GetOneCategoryByCategoryId(id, trackChanges);

        if (entity is null)
            throw new CategoryNotFoundException(id);

        return _mapper.Map<CategoryDto>(entity);
    }

    public async  Task<CategoryDto> CreateOneCategory(InsertCategoryDto category, bool trackChanges)
    {
        var entity = _mapper.Map<Category>(category);

         _repositoryManager.Category.CreateOneCategory(entity);

         await _repositoryManager.SaveAsync();

         return _mapper.Map<CategoryDto>(entity);
    }

    public async  Task UpdateOneCategoryAsync(int id, UpdateCategoryDto category, bool trackChanges)
    {
        if(category is null)
            throw new ArgumentNullException(nameof(category));

        var entity = await _repositoryManager.Category.GetOneCategoryByCategoryId(id, trackChanges);

        if(entity is null)
            throw new CategoryNotFoundException(id);

        _repositoryManager.Category.UpdateOneRepository(_mapper.Map<Category>(category));
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteOneCategoryAsync(int id, bool trackChanges)
    {
        var entity = await _repositoryManager.Category.GetOneCategoryByCategoryId(id, trackChanges);

        if(entity is null)
            throw new CategoryNotFoundException(id);

        _repositoryManager.Category.DeleteOneCategory(entity);
        await _repositoryManager.SaveAsync();
    }
}