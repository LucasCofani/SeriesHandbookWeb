using Microsoft.AspNetCore.Components.Authorization;
using SeriesHandbookShared.Models.TMDB;
using SeriesHandbookShared.Models.TMDB.Movies;
using SeriesHandbookShared.Models.TMDB.Search;
using SeriesHandbookShared.Models.TMDB.Series;
using SeriesHandbookSPA.Authentication;
using SeriesHandbookSPA.Network;
using System;
using System.Threading.Tasks;

namespace SeriesHandbookSPA.Services
{
    public class SeriesHandbookHandler
    {
        public enum SeriesEvents
        {
            GetMovieDetail,
            GetMovieSearch,
            GetMovieSearchNext,
            GetMovieSearchPrevious,
            GetMovieSearchPage,
            GetSerieDetail,
            GetSerieSearch,
            GetSerieSearchNext,
            GetSerieSearchPrevious,
            GetSerieSearchPage
        }
        public string Query { get; set; }
        public string Id { get; set; }
        public string Page { get; set; }
        public ResponseWrapper<MoviesWrapper> Movies{ get; private set; }
        public ResponseWrapper<SeriesWrapper> Series { get; private set; }
        public ResponseWrapper<SearchWrapper> Search { get; private set; }

        private readonly SeriesHandbookApi _api;
        private readonly AuthenticationStateProvider authprovider;
        public SeriesHandbookHandler(SeriesHandbookApi api, AuthenticationStateProvider authprovider)
        {
            _api = api;
            this.authprovider = authprovider;
        }

        public async Task EventHandler(SeriesEvents seriesEvent)
        {
            try
            {
                switch (seriesEvent)
                {
                    case SeriesEvents.GetMovieDetail:
                        Movies = await _api.GetMovieDetail(Id);
                        break;
                    case SeriesEvents.GetMovieSearch:
                        Search = await _api.GetMovieSearch(Query);
                        break;
                    case SeriesEvents.GetMovieSearchNext:
                        Search = await _api.GetMovieSearchNext(Query);
                        break;
                    case SeriesEvents.GetMovieSearchPrevious:
                        Search = await _api.GetMovieSearchPrevious(Query);
                        break;
                    case SeriesEvents.GetMovieSearchPage:
                        Search = await _api.GetMovieSearchPage(Query,Page);
                        break;
                    case SeriesEvents.GetSerieDetail:
                        Series = await _api.GetSerieDetail(Id);
                        break;
                    case SeriesEvents.GetSerieSearch:
                        Search = await _api.GetSerieSearch(Query);
                        break;
                    case SeriesEvents.GetSerieSearchNext:
                        Search = await _api.GetSerieSearchNext(Query);
                        break;
                    case SeriesEvents.GetSerieSearchPrevious:
                        Search = await _api.GetSerieSearchPrevious(Query);
                        break;
                    case SeriesEvents.GetSerieSearchPage:
                        Search = await _api.GetSerieSearchPage(Query,Page);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Response status code does not indicate success: 401 (Unauthorized).")
                    await ((CustomAuthenticationStateProvider)authprovider).MarkUserAsLoggedOut();
                else
                    Console.WriteLine(e.Message);
            }
            
        }
    }
}
