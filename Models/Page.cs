namespace Market.Models
{
    public class Page<T>
    {
        public List<T>? Items { get; set; }
        public int Total { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
    }
}