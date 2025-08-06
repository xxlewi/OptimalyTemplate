using OptimalyTemplate.DataLayer.Data;
using OptimalyTemplate.DataLayer.Entities;
using OptimalyTemplate.DataLayer.Interfaces;

namespace OptimalyTemplate.DataLayer.Repositories;

/// <summary>
/// Repository implementace pro entity s int ID (dědí z BaseEntity)
/// Poskytuje backward compatibility s původním IRepository TEntity interface
/// </summary>
/// <typeparam name="TEntity">Typ entity dědící z BaseEntity (int ID)</typeparam>
public class Repository<TEntity> : BaseRepository<TEntity, int>, IRepository<TEntity> 
    where TEntity : BaseEntity
{
    public Repository(ApplicationDbContext context) : base(context)
    {
    }
}