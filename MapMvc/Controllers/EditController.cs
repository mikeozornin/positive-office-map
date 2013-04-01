using System.Web.Mvc;
using MapData;

namespace MapMvc.Controllers
{
	public class EditController : Controller
    {
		[System.Web.Http.HttpPost]
		[System.Web.Http.Authorize(Roles = "Manager")]
		public void Bind(string poiId, int objectId)
		{
			if (!string.IsNullOrEmpty(poiId))
				PoiDataHelper.BindPoiToObject(poiId, objectId);
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.Authorize(Roles = "Manager")]
		public void Unbind(string poiId)
		{
			if (!string.IsNullOrEmpty(poiId))
				PoiDataHelper.UnbindPoiFromAllObjects(poiId);
		}

    }
}
