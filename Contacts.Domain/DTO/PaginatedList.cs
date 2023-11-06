namespace Contacts.Domain.DTO
{
    public class PaginatedList<T>
    {
        public PaginatedList()
        {
            Lista = new List<T>();
        }

        public List<T> Lista { get; set; }
        public int TotalRegistros { get; set; }
    }
}
