namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string meaasge) : base(meaasge)
        {
        }
    }
}