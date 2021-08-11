using Refit;
using System.Threading.Tasks;
using SeriesHandbookShared.Models.TMDB;
using SeriesHandbookShared.Models.TMDB.Movies;
using SeriesHandbookShared.Models.TMDB.Series;
using SeriesHandbookShared.Models.TMDB.Search;

namespace SeriesHandbookSPA.Network
{
    public interface SeriesHandbookApi
    {
        [Get("/api/Movies/Detail/{id}")]
        Task<ResponseWrapper<MoviesWrapper>> GetMovieDetail(string id);
        [Get("/api/Movies/{query}")]
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearch(string query);
        [Get("/api/Movies/{query}/Next")]
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearchNext(string query);
        [Get("/api/Movies/{query}/Previous")]
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearchPrevious(string query);
        [Get("/api/Movies/{query}/{page}")]
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearchPage(string query,string page);

        [Get("/api/Series/Detail/{id}")]
        Task<ResponseWrapper<SeriesWrapper>> GetSerieDetail(string id);
        [Get("/api/Series/{query}")]
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearch(string query);
        [Get("/api/Series/{query}/Next")]
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearchNext(string query);
        [Get("/api/Series/{query}/Previous")]
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearchPrevious(string query);
        [Get("/api/Series/{query}/{page}")]
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearchPage(string query, string page);
    }
}
