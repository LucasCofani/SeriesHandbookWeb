using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SeriesHandbookAPI.Models;
using System.Threading.Tasks;

namespace SeriesHandbookAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FirebaseController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public FirebaseController(IOptions<FirebaseConfig> options)
        {
            _db = FirestoreDb.Create(options.Value.ProjectId);
        }
        [HttpGet]
        public async Task<IActionResult> Get()        {
            
            var docRef = _db.Collection("RandomTest").Document("Version");
            var snap = await docRef.GetSnapshotAsync();
            if (snap.Exists)
            {
                var version = snap.ConvertTo<Version>();
                return Ok(version);
            }
            else
            {
                return NoContent();
            }
        }
    }
    [FirestoreData]
    public class Version
    {
        [FirestoreProperty(name:"Version")]
        public string VersionNumber { get; set; }
    }
}
