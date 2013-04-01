using MapData;
using System;
using System.Linq;
using System.Threading;

namespace PTMapDataUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("{0}: DataUpdater Started.", DateTime.Now);

			Console.WriteLine("{0}: Get users from portal.", DateTime.Now);
			var poisFromPortal = PoiDataHelper.GetPersonalInfoFromPortal();
			Console.WriteLine("{0}: {1} users found.", DateTime.Now, poisFromPortal.Count);

			Console.WriteLine("{0}: Get users from db.", DateTime.Now);
			var poisFromDb = PoiDataHelper.GetPoiList(MapObjectType.Person);
			Console.WriteLine("{0}: {1} users found.", DateTime.Now, poisFromDb.Count);

			Console.WriteLine("{0}: Deleting not actual data.", DateTime.Now);
			// Удаляем тех, кого больше нет на портале
			foreach (var poi in poisFromDb.Where(poi => poisFromPortal.All(p => p.Id != poi.Id)))
			{
				PoiDataHelper.DeletePoi(poi);
				Console.WriteLine("{0}: {1} deleted.", DateTime.Now, poi.Id);
			}

			Console.WriteLine("{0}: Updating data.", DateTime.Now);
			// Обновляем тех, кто есть / добавляем новеньких
			foreach (var poi in poisFromPortal)
			{
				if (PoiDataHelper.GetPoi(poi.Id) != null)
				{
					PoiDataHelper.UpdatePoi(poi);
					//Console.WriteLine("{0}: {1} updated.", DateTime.Now, poi.Id);
				}
				else
				{
					PoiDataHelper.CreatePoi(poi);
					Console.WriteLine("{0}: {1} added.", DateTime.Now, poi.Id);
				}
			}

			Console.WriteLine("{0}: Update finished.", DateTime.Now);
			Thread.Sleep(10000);
		}
	}
}
