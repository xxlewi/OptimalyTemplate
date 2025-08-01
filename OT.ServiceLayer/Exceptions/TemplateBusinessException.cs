namespace OT.ServiceLayer.Exceptions;

/// <summary>
/// Template-specific business logic exception following CRM pattern
/// Represents business rule violations specific to template domain
/// Template exception - remove in production or rename to your domain
/// </summary>
public class TemplateBusinessException : BusinessException
{
    /// <summary>
    /// Template entity type involved in business rule violation
    /// </summary>
    public string EntityType { get; }

    /// <summary>
    /// Entity ID involved in business rule violation (if applicable)
    /// </summary>
    public object? EntityId { get; }

    /// <summary>
    /// Business rule that was violated
    /// </summary>
    public string BusinessRule { get; }

    public TemplateBusinessException(string entityType, string businessRule, string message) 
        : base(message, businessRule)
    {
        EntityType = entityType;
        BusinessRule = businessRule;
    }

    public TemplateBusinessException(string entityType, object entityId, string businessRule, string message) 
        : base(message, businessRule)
    {
        EntityType = entityType;
        EntityId = entityId;
        BusinessRule = businessRule;
    }

    public TemplateBusinessException(string entityType, string businessRule, string message, Exception innerException) 
        : base(message, innerException, businessRule)
    {
        EntityType = entityType;
        BusinessRule = businessRule;
    }

    /// <summary>
    /// Create business exception for product stock validation
    /// </summary>
    public static TemplateBusinessException ProductOutOfStock(int productId, string productName)
    {
        return new TemplateBusinessException(
            "TemplateProduct", 
            productId, 
            "STOCK_VALIDATION",
            $"Produkt '{productName}' není skladem.");
    }

    /// <summary>
    /// Create business exception for product pricing validation
    /// </summary>
    public static TemplateBusinessException InvalidSalePrice(int productId, decimal price, decimal salePrice)
    {
        return new TemplateBusinessException(
            "TemplateProduct", 
            productId, 
            "PRICE_VALIDATION",
            $"Akční cena ({salePrice:C}) musí být nižší než běžná cena ({price:C}).");
    }

    /// <summary>
    /// Create business exception for category deletion with products
    /// </summary>
    public static TemplateBusinessException CategoryHasProducts(int categoryId, string categoryName, int productCount)
    {
        return new TemplateBusinessException(
            "TemplateCategory", 
            categoryId, 
            "CATEGORY_DELETION",
            $"Kategorii '{categoryName}' nelze smazat, obsahuje {productCount} produktů. Nejprve přesuňte nebo smažte produkty.");
    }

    /// <summary>
    /// Create business exception for duplicate SKU
    /// </summary>
    public static TemplateBusinessException DuplicateSku(string sku, int? existingProductId = null)
    {
        var message = existingProductId.HasValue 
            ? $"SKU '{sku}' již existuje u produktu ID {existingProductId}."
            : $"SKU '{sku}' již existuje u jiného produktu.";
            
        return new TemplateBusinessException(
            "TemplateProduct", 
            "SKU_UNIQUENESS",
            message);
    }

    /// <summary>
    /// Create business exception for inactive category assignment
    /// </summary>
    public static TemplateBusinessException InactiveCategoryAssignment(int categoryId, string categoryName)
    {
        return new TemplateBusinessException(
            "TemplateProduct", 
            "CATEGORY_VALIDATION",
            $"Nelze zařadit produkt do neaktivní kategorie '{categoryName}'.");
    }

    /// <summary>
    /// Create business exception for product activation without category
    /// </summary>
    public static TemplateBusinessException ProductActivationWithoutCategory(int productId)
    {
        return new TemplateBusinessException(
            "TemplateProduct", 
            productId, 
            "ACTIVATION_VALIDATION",
            "Nelze aktivovat produkt bez přiřazené kategorie.");
    }

    /// <summary>
    /// Create business exception for bulk operation limits
    /// </summary>
    public static TemplateBusinessException BulkOperationLimit(string operation, int requestedCount, int maxAllowed)
    {
        return new TemplateBusinessException(
            "BulkOperation", 
            "BULK_LIMIT",
            $"Operaci '{operation}' nelze provést pro {requestedCount} položek. Maximum je {maxAllowed}.");
    }
}