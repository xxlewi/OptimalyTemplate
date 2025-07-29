namespace OT.ServiceLayer.DTOs;

/// <summary>
/// Generický base DTO s podporou různých typů ID
/// </summary>
/// <typeparam name="TKey">Typ primárního klíče</typeparam>
public abstract class BaseDto<TKey>
{
    public TKey Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Base DTO pro entity s int ID (backward compatibility)
/// </summary>
public abstract class BaseDto : BaseDto<int>
{
}