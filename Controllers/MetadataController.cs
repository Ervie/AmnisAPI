using API.Model;
using API.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    public class MetadataController : Controller
    {
        private readonly ICollection<RadioStation> _radioStations;

        private readonly JSONSerializer _serializer;


        public MetadataController(JSONSerializer serializer)
        {
            _serializer = serializer;

            _radioStations = _serializer.LoadFromFile<RadioStation>("radioStationList.json");
        }

        // GET api/metadata
        [HttpGet]
        public ActionResult Get()
        {
            return Json(MetadataWorker.SendRequest("http://radio.vgmradio.com:8040/stream"));
        }

        // GET api/metadata/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            RadioStation selectedStation = _radioStations.FirstOrDefault(x => x.Id.Equals(id));

            return Json(selectedStation == null ? new SongMetadata() : MetadataWorker.SendRequest(selectedStation.ChannelUrl));
        }

        // POST api/metadata
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/metadata/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/metadata/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
