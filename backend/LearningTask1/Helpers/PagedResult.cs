namespace LearningTask1.Helpers
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Data { get; set; } = new List<T>();

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
