namespace APICatalogo.Pagination
{
    public class PagedList<T> : List<T> where T : class
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }


        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;


        public PagedList(List<T> items, int count, int pageNumber, int pagesize)
        {
            TotalCount = count;
            PageSize = pagesize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pagesize);
            AddRange(items);

        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber,  int pagesize)
        {
            var count  = source.Count();
            var items = source.Skip((pageNumber - 1) * pagesize).Take(pagesize).ToList();

            return new PagedList<T>(items, count, pageNumber, pagesize);
        }

    }
}
