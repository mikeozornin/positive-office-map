using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MapData
{
	public static class ObjectDataHelper
	{
		public static MapObject GetObject(int objectId)
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Object_GetList", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				cmd.Parameters.AddWithValue("@ObjectId", objectId);

				sc.Open();
				var reader = cmd.ExecuteReader();
				if (reader.Read())
					return FillObjectFromReader(reader);
			}
			return null;
		}

		public static List<MapObject> GetObjectList(string poiId)
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Object_GetList", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };
				cmd.Parameters.AddWithValue("@PoiId", poiId);

				sc.Open();
				var reader = cmd.ExecuteReader();
				var poiList = new List<MapObject>();
				while (reader.Read())
					poiList.Add(FillObjectFromReader(reader));

				return poiList;
			}
		}

		public static List<MapObject> GetObjectList()
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Object_GetList", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				sc.Open();
				var reader = cmd.ExecuteReader();
				var poiList = new List<MapObject>();
				while (reader.Read())
					poiList.Add(FillObjectFromReader(reader));

				return poiList;
			}
		}

		private static MapObject FillObjectFromReader(IDataReader reader)
		{
			return new MapObject
				       {
					       Id = (int)reader["ObjectId"],
					       Type = reader["Type"].ToString(),
					       Coords = reader["Coords"].ToString()
				       };
		}
	}
}