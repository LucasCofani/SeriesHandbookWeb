using Refit;
using SeriesHandbookShared.Models.TMDB.Movies;
using SeriesHandbookShared.Models.TMDB.Search;
using SeriesHandbookShared.Models.TMDB.Series;
using System.Threading.Tasks;

namespace SeriesHandbookAPI.Network
{
    public interface TMDBApi
    {
        [Get("/tv/{id}")]
        Task<SeriesWrapper> GetSerieDetails(
            string id,
            string api_key,
            string language = "pt-BR"
            );

        [Get("/search/tv")]
        Task<SearchWrapper> GetSerieSearch(
            string query,
            string api_key,
            string page = "1",
            string language = "pt-BR"
            );

        [Get("/movie/{id}")]
        Task<MoviesWrapper> GetMovieDetails(
            string id,
            string api_key,
            string language = "pt-BR"
            );

        [Get("/search/movie")]
        Task<SearchWrapper> GetMovieSearch(
            string query,
            string api_key,
            string page = "1",
            string language = "pt-BR"
            );
    }
}
