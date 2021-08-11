using System.Collections.Generic;

namespace SeriesHandbookShared.Models.TMDB.Search
{
    public class SearchWrapper
    {
        public int page { get; set; }
        public List<Result> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }
}
