using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeriesHandbookAPI.Repository;
using System.Threading.Tasks;

namespace SeriesHandbookAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesRepository _repo;

        public SeriesController(ISeriesRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetails(int id)        
        {
            var res = await _repo.GetDetail(id);
            if (res.ErrorMsg == null)            
                return Ok(res);            
            else
                return StatusCode(412, res);
        }

        
        [HttpGet("{query}")]
        public async Task<IActionResult> GetSearch(string query)
        {
            if (query == "revoke")
                return StatusCode(401);
            var res = await _repo.Search(query);
            if (res.ErrorMsg == null)
                return Ok(res);
            else
                return StatusCode(412, res);
        }

        [HttpGet("{query}/Next")]
        public async Task<IActionResult> GetSearchNext(string query)
        {
            var res = await _repo.SearchNextPage(query);
            if (res.ErrorMsg == null)
                return Ok(res);
            else
                return StatusCode(412, res);
        }

        [HttpGet("{query}/Previous")]
        public async Task<IActionResult> GetSearchPrevious(string query)
        {
            var res = await _repo.SearchNextPage(query);
            if (res.ErrorMsg == null)
                return Ok(res);
            else
                return StatusCode(412, res);
        }

        [HttpGet("{query}/{page}")]
        public async Task<IActionResult> GetSearchPrevious(string query,int page)
        {
            var res = await _repo.SearchPage(query, page);
            if (res.ErrorMsg == null)
                return Ok(res);
            else
                return StatusCode(412, res);
        }

    }
}
