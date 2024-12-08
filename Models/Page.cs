namespace Market.Models
{
    public class Page<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}