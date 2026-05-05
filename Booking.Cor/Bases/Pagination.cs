namespace Restaurant.Cor.Bases
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}

