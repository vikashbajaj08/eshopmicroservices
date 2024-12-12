namespace BuildingBlocks.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) 
        {

        }

        public NotFoundException(string message, Guid key) : base($"Entity \"{message}\" ({key} was not found)") 
        {
        }
    }
}
