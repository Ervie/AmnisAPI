using API.Model;
using API.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    public class ChannelController : Controller
    {
        private readonly ICollection<RadioStation> _radioStations;

        private readonly JSONSerializer _serializer;

        public ChannelController(JSONSerializer serializer)
        {
            _serializer = serializer;

            _radioStations = _serializer.LoadFromFile<RadioStation>("radioStationList.json");
        }

        // GET api/channel
        [HttpGet]
        public JsonResult Get()
        {
            return Json(_radioStations);
        }

        // GET api/channel/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            RadioStation selectedChannel = _radioStations.FirstOrDefault(x => x.Id.Equals(id));

            return Json(selectedChannel);
        }

        // POST api/channel
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/channel/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/channel/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
