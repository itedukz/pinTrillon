using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Core.Interfaces;
using System.Linq.Expressions;
using System.Text.Json;

namespace ms.MainApi.Core.DataAccess;

public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext, new()
{
    #region DI
    private readonly TContext context;
    private readonly IAuthInformationRepository _authInformationRepository;

    public EntityRepositoryBase(TContext _context, IAuthInformationRepository authInformationRepository)
    {
        context = _context;
        _authInformationRepository = authInformationRepository;
    }
    #endregion


    public async Task<List<TEntity>> GetAllQueryAsync(string? Query = null)
    {
        var jsonDocument = JsonDocument.Parse(Query);
        var jsonExpressionParser = new JsonExpressionParser();
        var filter = jsonExpressionParser.ParseExpressionOf<TEntity>(jsonDocument);

        return await GetAllAsync(filter);
    }
    

    public List<TEntity> GetAllQuery(ref int totalItems, int page, int pageSize, string? Query = null)
    {
        var jsonDocument = JsonDocument.Parse(Query);
        var jsonExpressionParser = new JsonExpressionParser();
        var filter = jsonExpressionParser.ParseExpressionOf<TEntity>(jsonDocument);

        List<TEntity> entities = GetAll(filter);

        totalItems = entities.Count();
        pageSize = pageSize > 0 ? pageSize : 10;
        page = page > 0 ? page - 1 : 0;

        return entities.Skip(page * pageSize).Take(pageSize).ToList();
    }

    public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null ?
            context.Set<TEntity>().OrderByDescending(o => o.id).ToList() :
            context.Set<TEntity>().Where(filter).OrderByDescending(o => o.id).ToList();
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        => context.Set<TEntity>().Where(filter).OrderByDescending(o => o.id).FirstOrDefault();

    public bool Any(Expression<Func<TEntity, bool>> filter) => context.Set<TEntity>().Any(filter);

    public void Add(TEntity entity)
    {
        var now = DateTime.Now;
        int currentUserId = _authInformationRepository.GetUserId();

        if (entity is IEntity creatableEntity)
        {
            creatableEntity.createdAt = now;
            creatableEntity.createdBy = currentUserId;
        }

        context.Entry(entity).State = EntityState.Added;
        context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        var now = DateTime.Now;
        int currentUserId = _authInformationRepository.GetUserId();

        if (entity is IEntity updatableEntity)
        {
            updatableEntity.updatedAt = now;
            updatableEntity.updatedBy = currentUserId;
        }

        context.Entry(entity).State = EntityState.Modified;
        context.SaveChanges();
    }

    #region Delete
    public void Delete(TEntity entity)
    {
        var now = DateTime.Now;
        int currentUserId = _authInformationRepository.GetUserId();

        entity.isDeleted = true;
        entity.deletedAt = now;
        entity.deletedBy = currentUserId;

        context.Entry(entity).State = EntityState.Deleted;
        context.SaveChanges();
    }
    #endregion

    public int Count(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null ? context.Set<TEntity>().Count() : context.Set<TEntity>().Where(filter).Count();
    }


    // async
    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null
            ? await context.Set<TEntity>().OrderByDescending(o => o.id).ToListAsync()
            : await context.Set<TEntity>().Where(filter).OrderByDescending(o => o.id).ToListAsync();
    }

    public async Task<List<TEntity>> GetAllPaginationAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? filter = null)
    {
        pageSize = pageSize > 0 ? pageSize : 10;
        page = page > 0 ? page - 1 : 0;

        return filter == null
            ? await context.Set<TEntity>().OrderByDescending(o => o.id).Skip(page * pageSize).Take(pageSize).ToListAsync()
            : await context.Set<TEntity>().Where(filter).OrderByDescending(o => o.id).Skip(page * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        => await context.Set<TEntity>().Where(filter).OrderByDescending(o => o.id).FirstOrDefaultAsync();

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        => await context.Set<TEntity>().AnyAsync(filter);

    public async Task AddAsync(TEntity entity)
    {
        var now = DateTime.Now;
        int currentUserId = _authInformationRepository.GetUserId();

        if (entity is IEntity creatableEntity)
        {
            creatableEntity.createdAt = now;
            creatableEntity.createdBy = currentUserId;
        }

        context.Entry(entity).State = EntityState.Added;
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var now = DateTime.Now;
        int currentUserId = _authInformationRepository.GetUserId();

        if (entity is IEntity updatableEntity)
        {
            updatableEntity.updatedAt = now;
            updatableEntity.updatedBy = currentUserId;
        }

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    #region Delete
    public async Task DeleteAsync(TEntity entity)
    {
        var now = DateTime.Now;
        int currentUserId = _authInformationRepository.GetUserId();

        entity.isDeleted = true;
        entity.deletedAt = now;
        entity.deletedBy = currentUserId;


        context.Entry(entity).State = EntityState.Deleted;
        await context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(List<TEntity> entities)
    {
        var now = DateTime.Now;
        int currentUserId = _authInformationRepository.GetUserId();
        foreach (IEntity deletableEntity in entities)
        {
            deletableEntity.isDeleted = true;
            deletableEntity.deletedAt = now;
            deletableEntity.deletedBy = currentUserId;
        }

        context.RemoveRange(entities);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        if (filter != null)
        {
            List<TEntity> deleteEntities = await context.Set<TEntity>().Where(filter).ToListAsync();

            var now = DateTime.Now;
            int currentUserId = _authInformationRepository.GetUserId();
            foreach (IEntity deletableEntity in deleteEntities)
            {
                deletableEntity.isDeleted = true;
                deletableEntity.deletedAt = now;
                deletableEntity.deletedBy = currentUserId;
            }

            context.RemoveRange(deleteEntities);
            await context.SaveChangesAsync();
        }
    }
    #endregion

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null ? context.Set<TEntity>().CountAsync() : context.Set<TEntity>().Where(filter).CountAsync();
    }


}
