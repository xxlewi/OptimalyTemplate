using OT.DataLayer.Interfaces;

namespace OT.DataLayer.Entities;

public abstract class BaseEntity : IBaseEntity<int>
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}