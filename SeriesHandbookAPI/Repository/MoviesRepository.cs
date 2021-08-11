using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using SeriesHandbookAPI.Models;
using SeriesHandbookAPI.Network;
using SeriesHandbookShared.Models.TMDB;
using SeriesHandbookShared.Models.TMDB.Movies;
using SeriesHandbookShared.Models.TMDB.Search;
using System;
using System.Threading.Tasks;

namespace SeriesHandbookAPI.Repository
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly FirestoreDb _db;
        private readonly TMDBApi _api;
        private static string _apiKey = null;
        private static int Page = 1;
        private static string Query = "";
        private static SearchWrapper PRes = null;
        public MoviesRepository(IOptions<FirebaseConfig> options, TMDBApi api)
        {
            _db = FirestoreDb.Create(options.Value.ProjectId); ;
            _api = api;
        }
        
        public async Task<ResponseWrapper<MoviesWrapper>> GetDetail(int key)
        {
            await GetApiFromDb();
            if (_apiKey != "") {
                try
                {
                    var res = await _api.GetMovieDetails(key.ToString(), _apiKey);
                    return ResponseWrapper<MoviesWrapper>.Ok(res);
                }
                catch (Exception e)
                {
                    return ResponseWrapper<MoviesWrapper>.Error(e.Message);
                }
            }
            return ResponseWrapper<MoviesWrapper>.Error("API key not found.");
        }

        public async Task<ResponseWrapper<SearchWrapper>> Search(string query)
        {
            await GetApiFromDb();
            Page = 1;
            Query = query;
            if (_apiKey != "")
            {
                try
                {
                    PRes = await _api.GetMovieSearch(Query, _apiKey);
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);
                }
                catch (Exception e) { 
                    return ResponseWrapper<SearchWrapper>.Error(e.Message);
                }
            }
            return ResponseWrapper<SearchWrapper>.Error("API key not found.");
        }

        public async Task<ResponseWrapper<SearchWrapper>> SearchNextPage(string query)
        {
            await GetApiFromDb();
            if (_apiKey != "")
            {
                if (query != Query)
                    return await Search(query);

                if (Page == PRes.total_pages)                
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);
                
                try
                {
                    Page = Page + 1;
                    PRes = await _api.GetMovieSearch(Query, _apiKey, Page.ToString());
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);
                }
                catch (Exception e) { 
                    return ResponseWrapper<SearchWrapper>.Error(e.Message);
                    }

                }
            return ResponseWrapper<SearchWrapper>.Error("API key not found.");
        }

        public async Task<ResponseWrapper<SearchWrapper>> SearchPreviousPage(string query)
        {
            await GetApiFromDb();
            if (_apiKey != "")
            {
                if (query != Query)
                    return await Search(query);

                if (Page == 1)                
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);
                
                try
                {
                    Page = Page - 1;
                    PRes = await _api.GetMovieSearch(Query, _apiKey, Page.ToString());
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);
                }
                catch (Exception e)
                {
                    return ResponseWrapper<SearchWrapper>.Error(e.Message);
                }
            }
            return ResponseWrapper<SearchWrapper>.Error("API key not found.");
        }
        public async Task<ResponseWrapper<SearchWrapper>> SearchPage(string query,int page)
        {
            await GetApiFromDb();
            if (_apiKey != "")
            {
                if (query != Query)
                    return await Search(query);

                if (page < 1 || page > PRes.total_pages)
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);

                try
                {
                    Page = page;
                    PRes = await _api.GetMovieSearch(Query, _apiKey, Page.ToString());
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);
                }
                catch (Exception e)
                {
                    return ResponseWrapper<SearchWrapper>.Error(e.Message);
                }
            }
            return ResponseWrapper<SearchWrapper>.Error("API key not found.");
        }

        private async Task GetApiFromDb()
        {
            if (_apiKey == null) { 
                var docRef = _db.Collection("AppSettings").Document("ApiKey");
                var snap = await docRef.GetSnapshotAsync();
                if (snap.Exists)
                    _apiKey = snap.GetValue<string>("Key");
                else
                    _apiKey = "";
            }
        }

    }
    public interface IMoviesRepository
    {
        Task<ResponseWrapper<SearchWrapper>> Search(string query);
        Task<ResponseWrapper<SearchWrapper>> SearchNextPage(string query);
        Task<ResponseWrapper<SearchWrapper>> SearchPreviousPage(string query);
        Task<ResponseWrapper<SearchWrapper>> SearchPage(string query,int page);
        Task<ResponseWrapper<MoviesWrapper>> GetDetail(int key);
    }
}
