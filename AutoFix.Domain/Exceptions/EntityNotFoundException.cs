namespace AutoFix.Domain.Exceptions
{
    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string entityName, int id)
            : base($"{entityName} con Id {id} no fue encontrado.")
        {
        }
    }
}
