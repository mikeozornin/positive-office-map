using MapData;
using System.Collections.Generic;
using System.Web.Http;

namespace MapMvc.Controllers
{
	public class PoiController : ApiController
    {
        // GET api/poi
		[Authorize]
        public IEnumerable<MapPoi> Get()
        {
			return PoiDataHelper.GetPoiList().ToArray();
        }

		/*
        // POST api/poi
		public void Post()
        {
        }
		*/

    }
}
