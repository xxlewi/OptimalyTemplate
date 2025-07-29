namespace OT.ServiceLayer.Exceptions;

public class NotFoundException : BusinessException
{
    public NotFoundException(string message) : base(message, "NOT_FOUND")
    {
    }

    public NotFoundException(string entityName, object key) 
        : base($"Entita '{entityName}' s klíčem '{key}' nebyla nalezena.", "NOT_FOUND")
    {
    }

    public NotFoundException(Type entityType, object key)
        : base($"Entita typu '{entityType.Name}' s klíčem '{key}' nebyla nalezena.", "NOT_FOUND")
    {
    }
}