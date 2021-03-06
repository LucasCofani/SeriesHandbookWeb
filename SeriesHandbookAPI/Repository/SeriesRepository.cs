using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using SeriesHandbookAPI.Models;
using SeriesHandbookAPI.Network;
using SeriesHandbookShared.Models.TMDB;
using SeriesHandbookShared.Models.TMDB.Search;
using SeriesHandbookShared.Models.TMDB.Series;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Collections;

namespace SeriesHandbookAPI.Repository
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly FirestoreDb _db;
        private readonly TMDBApi _api;
        private readonly IHttpContextAccessor _http;
        private static string _apiKey = null;
        private static int Page = 1;
        private static string Query = "";
        private static SearchWrapper PRes = null;
        public SeriesRepository(IOptions<FirebaseConfig> options, TMDBApi api, IHttpContextAccessor http)
        {
            _db = FirestoreDb.Create(options.Value.ProjectId); ;
            _api = api;
            _http = http;
        }

        public async Task<ResponseWrapper<SeriesWrapper>> GetDetail(int key)
        {
            await GetApiFromDb();
            if (_apiKey != "") {
                try
                {
                    var res = await _api.GetSerieDetails(key.ToString(), _apiKey);
                    return ResponseWrapper<SeriesWrapper>.Ok(res);
                }
                catch (Exception e)
                {
                    return ResponseWrapper<SeriesWrapper>.Error(e.Message);
                }
            }
            return ResponseWrapper<SeriesWrapper>.Error("API key not found.");
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
                    PRes = await _api.GetSerieSearch(Query, _apiKey);
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
                    PRes = await _api.GetSerieSearch(Query, _apiKey, Page.ToString());
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
                    PRes = await _api.GetSerieSearch(Query, _apiKey, Page.ToString());
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
                    PRes = await _api.GetSerieSearch(Query, _apiKey, Page.ToString());
                    return ResponseWrapper<SearchWrapper>.Ok(PRes);
                }
                catch (Exception e)
                {
                    return ResponseWrapper<SearchWrapper>.Error(e.Message);
                }
            }
            return ResponseWrapper<SearchWrapper>.Error("API key not found.");
        }
        public async Task SetBookmark(int key)
        {
            var doc = _db.Collection("UserBookmarks").Document(_http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var snap = await doc.GetSnapshotAsync();
            if (snap.Exists) {
                var tempDb = snap.ConvertTo<Dictionary<string, ArrayList>>();

                var converted = tempDb["FavSeries"].ToArray();
                
                if (converted.FirstOrDefault(p => p.ToString() == key.ToString()) == null) {
                    tempDb["FavSeries"].Add(key.ToString());
                }
                else
                {
                    tempDb["FavSeries"].Remove(key.ToString());
                }
                await doc.SetAsync(tempDb);
            }
            else
            {
                var tempdb = new Dictionary<string,object>();
                
                var favList = new ArrayList();
                
                favList.Add(key.ToString());
                tempdb.Add("FavMov", favList);
                tempdb.Add("FavSeries", favList);

                await doc.SetAsync(tempdb);
            }
        }
        public async Task<bool> GetBookmarkDetail(int key)
        {
            var doc = _db.Collection("UserBookmarks").Document(_http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var snap = await doc.GetSnapshotAsync();
            if (snap.Exists)
            {
                var tempDb = snap.ConvertTo<Dictionary<string, ArrayList>>();
                var converted = tempDb["FavSeries"].ToArray();
                if (converted.FirstOrDefault(p => p.ToString() == key.ToString()) != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<ResponseWrapper<SeriesWrapper>>> GetBookmarkAll()
        {
            var doc = _db.Collection("UserBookmarks").Document(_http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var snap = await doc.GetSnapshotAsync();
            if (snap.Exists)
            {
                var saida = new List<ResponseWrapper<SeriesWrapper>>();
                var tempDb = snap.ConvertTo<Dictionary<string, ArrayList>>();
                foreach (var item in tempDb["FavSeries"].ToArray())
                {
                    saida.Add(await GetDetail(Int32.Parse(item.ToString())));
                }
                return saida;
            }
            return default;
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
    public interface ISeriesRepository
    {
        Task<ResponseWrapper<SearchWrapper>> Search(string query);
        Task<ResponseWrapper<SearchWrapper>> SearchNextPage(string query);
        Task<ResponseWrapper<SearchWrapper>> SearchPreviousPage(string query);
        Task<ResponseWrapper<SearchWrapper>> SearchPage(string query,int page);
        Task<ResponseWrapper<SeriesWrapper>> GetDetail(int key);
        Task<bool> GetBookmarkDetail(int key);
        Task SetBookmark(int key);
        Task<List<ResponseWrapper<SeriesWrapper>>> GetBookmarkAll();
    }
}
