using MapData;
using System.Collections.Generic;
using System.Web.Http;

namespace MapMvc.Controllers
{
    public class ObjectController : ApiController
    {
        // GET api/object
		[Authorize]
		public IEnumerable<MapObject> Get()
        {
			return ObjectDataHelper.GetObjectList().ToArray();
        }

		/*
        // POST api/object
        public void Post([FromBody]string value)
        {
        }

        // PUT api/object/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/object/5
        public void Delete(int id)
        {
        }
		*/
    }
}
