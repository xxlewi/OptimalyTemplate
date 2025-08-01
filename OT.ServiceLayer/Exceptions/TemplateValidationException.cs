namespace OT.ServiceLayer.Exceptions;

/// <summary>
/// Template-specific validation exception following CRM pattern
/// Represents validation failures specific to template entities
/// Template exception - remove in production or rename to your domain
/// </summary>
public class TemplateValidationException : ValidationException
{
    /// <summary>
    /// Template entity type that failed validation
    /// </summary>
    public string EntityType { get; }

    /// <summary>
    /// Entity ID that failed validation (if applicable)
    /// </summary>
    public object? EntityId { get; }

    public TemplateValidationException(string entityType, string message) 
        : base(message)
    {
        EntityType = entityType;
    }

    public TemplateValidationException(string entityType, object entityId, string message) 
        : base(message)
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public TemplateValidationException(string entityType, string fieldName, string message) 
        : base(fieldName, message)
    {
        EntityType = entityType;
    }

    public TemplateValidationException(string entityType, object entityId, string fieldName, string message) 
        : base(fieldName, message)
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public TemplateValidationException(string entityType, Dictionary<string, string[]> errors) 
        : base(errors)
    {
        EntityType = entityType;
    }

    public TemplateValidationException(string entityType, object entityId, Dictionary<string, string[]> errors) 
        : base(errors)
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    /// <summary>
    /// Create template product validation exception
    /// </summary>
    public static TemplateValidationException ForProduct(string message)
    {
        return new TemplateValidationException("TemplateProduct", message);
    }

    /// <summary>
    /// Create template product validation exception with field
    /// </summary>
    public static TemplateValidationException ForProduct(string fieldName, string message)
    {
        return new TemplateValidationException("TemplateProduct", fieldName, message);
    }

    /// <summary>
    /// Create template product validation exception with ID
    /// </summary>
    public static TemplateValidationException ForProduct(int productId, string message)
    {
        return new TemplateValidationException("TemplateProduct", productId, message);
    }

    /// <summary>
    /// Create template product validation exception with ID and field
    /// </summary>
    public static TemplateValidationException ForProduct(int productId, string fieldName, string message)
    {
        return new TemplateValidationException("TemplateProduct", productId, fieldName, message);
    }

    /// <summary>
    /// Create template category validation exception
    /// </summary>
    public static TemplateValidationException ForCategory(string message)
    {
        return new TemplateValidationException("TemplateCategory", message);
    }

    /// <summary>
    /// Create template category validation exception with field
    /// </summary>
    public static TemplateValidationException ForCategory(string fieldName, string message)
    {
        return new TemplateValidationException("TemplateCategory", fieldName, message);
    }

    /// <summary>
    /// Create template category validation exception with ID
    /// </summary>
    public static TemplateValidationException ForCategory(int categoryId, string message)
    {
        return new TemplateValidationException("TemplateCategory", categoryId, message);
    }
}