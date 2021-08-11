using Microsoft.AspNetCore.Components.Authorization;
using SeriesHandbookShared.Models.TMDB;
using SeriesHandbookShared.Models.TMDB.Movies;
using SeriesHandbookShared.Models.TMDB.Search;
using SeriesHandbookShared.Models.TMDB.Series;
using SeriesHandbookSPA.Authentication;
using SeriesHandbookSPA.Network;
using System.Threading.Tasks;

namespace SeriesHandbookSPA.Services
{
    public class SeriesHandbookService : ISeriesHandbookService
    {
        private readonly SeriesHandbookApi _api;
        private readonly AuthenticationStateProvider authprovider;
        public SeriesHandbookService(SeriesHandbookApi api, AuthenticationStateProvider authprovider)
        {
            _api = api;
            this.authprovider = authprovider;
        }

        public async Task<ResponseWrapper<MoviesWrapper>> GetMovieDetail(string id)
        {
            try
            {
                return await _api.GetMovieDetail(id);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetMovieSearch(string query)
        {
            try
            {
                return await _api.GetMovieSearch(query);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetMovieSearchNext(string query)
        {
            try
            {
                return await _api.GetMovieSearchNext(query);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetMovieSearchPage(string query, string page)
        {
            try
            {
                return await _api.GetMovieSearchPage(query, page);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetMovieSearchPrevious(string query)
        {
            try
            {
                return await _api.GetMovieSearchPrevious(query);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SeriesWrapper>> GetSerieDetail(string id)
        {
            try
            {
                return await _api.GetSerieDetail(id);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetSerieSearch(string query)
        {
            try
            {
                return await _api.GetSerieSearch(query);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetSerieSearchNext(string query)
        {
            try
            {
                return await _api.GetSerieSearchNext(query);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetSerieSearchPage(string query, string page)
        {
            try
            {
                return await _api.GetSerieSearchPage(query, page);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }

        public async Task<ResponseWrapper<SearchWrapper>> GetSerieSearchPrevious(string query)
        {
            try
            {
                return await _api.GetSerieSearchPrevious(query);
            }
            catch (System.Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                {
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                }
            }
            return default;
        }
    }
    public interface ISeriesHandbookService
    {
        Task<ResponseWrapper<MoviesWrapper>> GetMovieDetail(string id);
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearch(string query);
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearchNext(string query);
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearchPrevious(string query);
        Task<ResponseWrapper<SearchWrapper>> GetMovieSearchPage(string query, string page);
        Task<ResponseWrapper<SeriesWrapper>> GetSerieDetail(string id);
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearch(string query);
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearchNext(string query);
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearchPrevious(string query);
        Task<ResponseWrapper<SearchWrapper>> GetSerieSearchPage(string query, string page);
        
    }
}
