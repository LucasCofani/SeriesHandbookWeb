using Microsoft.AspNetCore.Components.Authorization;
using SeriesHandbookShared.Models.TMDB;
using SeriesHandbookShared.Models.TMDB.Movies;
using SeriesHandbookShared.Models.TMDB.Search;
using SeriesHandbookShared.Models.TMDB.Series;
using SeriesHandbookSPA.Authentication;
using SeriesHandbookSPA.Network;
using System;
using System.Collections.Generic;
using System.Linq;
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
            SetMoviesBookmark,
            GetMoviesBookmark,
            GetMoviesBookmarkDetail,
            GetSerieDetail,
            GetSerieSearch,
            GetSerieSearchNext,
            GetSerieSearchPrevious,
            GetSerieSearchPage,
            SetSeriesBookmark,
            GetSeriesBookmark,
            GetSeriesBookmarkDetail,
        }
        public string Query { get; set; }
        public string Id { get; set; }
        public string Page { get; set; }
        public bool Bookmark { get; set; }
        public ResponseWrapper<MoviesWrapper> Movies{ get; private set; }
        public ResponseWrapper<SeriesWrapper> Series { get; private set; }
        public ResponseWrapper<SearchWrapper> Search { get; private set; }

        public List<ResponseWrapper<SeriesWrapper>> SeriesBookmark { get; private set; }
        public List<ResponseWrapper<MoviesWrapper>> MoviesBookmark { get; private set; }

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
                        if (MoviesBookmark != null)
                        {
                            var temp = MoviesBookmark.FirstOrDefault(p => p.Info.id.ToString() == Id);
                            if (temp != null)
                                Movies = temp;
                            else
                                Movies = await _api.GetMovieDetail(Id);
                        }
                        else
                            Movies = await _api.GetMovieDetail(Id);
                        Console.WriteLine(Movies);
                        Bookmark = await _api.GetMovieBookmarkDetail(Id);
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
                        Search = await _api.GetMovieSearchPage(Query, Page);
                        break;

                    case SeriesEvents.GetSerieDetail:
                        if (SeriesBookmark!= null) { 
                            var temp = SeriesBookmark.FirstOrDefault(p => p.Info.id.ToString() == Id);                       
                            if (temp != null)
                                Series = temp;
                            else 
                                Series = await _api.GetSerieDetail(Id);
                        }
                        else
                            Series = await _api.GetSerieDetail(Id);
                        
                        Bookmark = await _api.GetSerieBookmarkDetail(Id);
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
                    case SeriesEvents.SetSeriesBookmark:
                        await _api.SetSerieBookmark(Id);
                        Bookmark = await _api.GetSerieBookmarkDetail(Id);
                        break;
                    case SeriesEvents.GetSeriesBookmark:
                        SeriesBookmark = await _api.GetSerieBookmark();
                        break;
                    case SeriesEvents.GetSeriesBookmarkDetail:
                        Bookmark = await _api.GetSerieBookmarkDetail(Id);
                        break;

                    case SeriesEvents.SetMoviesBookmark:
                        await _api.SetMovieBookmark(Id);
                        Bookmark = await _api.GetMovieBookmarkDetail(Id);
                        break;
                    case SeriesEvents.GetMoviesBookmark:
                        MoviesBookmark = await _api.GetMovieBookmark();
                        break;
                    case SeriesEvents.GetMoviesBookmarkDetail:
                        Bookmark = await _api.GetMovieBookmarkDetail(Id);
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
