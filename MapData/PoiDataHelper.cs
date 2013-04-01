using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MapData
{
	public static class PoiDataHelper
	{
		public static List<MapPoi> GetPersonalInfoFromPortal()
		{
			var poiList = new List<MapPoi>();

			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_PortalData_Get", sc)
					          {
						          CommandType = CommandType.StoredProcedure
							  };
				sc.Open();
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string personName = string.Format("{0}{1}{2}", reader["LAST_NAME"] != DBNull.Value ? reader["LAST_NAME"] + " " : string.Empty,
					                                  reader["NAME"] != DBNull.Value ? reader["NAME"] + " " : string.Empty,
					                                  reader["SECOND_NAME"] != DBNull.Value ? reader["SECOND_NAME"] : string.Empty);

					poiList.Add(new MapPoi
						            {
							            Id = reader["LOGIN"].ToString(),
							            Name = personName.Trim(),
							            Title = reader["WORK_POSITION"].ToString(),
							            PortalId = (int)reader["ID"],
							            Email = reader["EMAIL"].ToString(),
							            ImageUrl = reader["PHOTO_URL"].ToString(),
							            Phone = reader["PERSONAL_MOBILE"].ToString(),
							            Type = MapObjectType.Person
						            });
				}
			}
			return poiList;
		}

		public static void CreatePoi(MapPoi poi)
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_Create", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				cmd.Parameters.AddWithValue("@PoiId", poi.Id);
				cmd.Parameters.AddWithValue("@Name", poi.Name);
				cmd.Parameters.AddWithValue("@Title", poi.Title);
				cmd.Parameters.AddWithValue("@PortalId", poi.PortalId);
				cmd.Parameters.AddWithValue("@Email", poi.Email);
				cmd.Parameters.AddWithValue("@ImageUrl", poi.ImageUrl);
				cmd.Parameters.AddWithValue("@Phone", poi.Phone);
				cmd.Parameters.AddWithValue("@Type", poi.Type);

				sc.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public static void UpdatePoi(MapPoi poi)
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_Update", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				cmd.Parameters.AddWithValue("@PoiId", poi.Id);
				cmd.Parameters.AddWithValue("@Name", poi.Name);
				cmd.Parameters.AddWithValue("@Title", poi.Title);
				cmd.Parameters.AddWithValue("@PortalId", poi.PortalId);
				cmd.Parameters.AddWithValue("@Email", poi.Email);
				cmd.Parameters.AddWithValue("@ImageUrl", poi.ImageUrl);
				cmd.Parameters.AddWithValue("@Phone", poi.Phone);
				cmd.Parameters.AddWithValue("@Type", poi.Type);

				sc.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public static void DeletePoi(MapPoi poi)
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_Delete", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				cmd.Parameters.AddWithValue("@PoiId", poi.Id);

				sc.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public static MapPoi GetPoi(string poiId)
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_GetList", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				cmd.Parameters.AddWithValue("@PoiId", poiId);

				sc.Open();
				var reader = cmd.ExecuteReader();
				if (reader.Read())
					return FillPoiFromReader(reader);
			}
			return null;
		}

		public static List<MapPoi> GetPoiList()
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_GetList", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				sc.Open();
				var reader = cmd.ExecuteReader();
				var poiList = new List<MapPoi>();
				while (reader.Read())
					poiList.Add(FillPoiFromReader(reader));

				return poiList;
			}
		}

		public static List<MapPoi> GetPoiList(MapObjectType poiType)
		{
			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_GetList", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };

				cmd.Parameters.AddWithValue("@PoiType", (int)poiType);
				sc.Open();
				var reader = cmd.ExecuteReader();
				var poiList = new List<MapPoi>();
				while (reader.Read())
					poiList.Add(FillPoiFromReader(reader));

				return poiList;
			}
		}

		private static MapPoi FillPoiFromReader(IDataReader reader)
		{
			return new MapPoi
				       {
					       Id = reader["PoiId"].ToString(),
					       Name = reader["Name"].ToString(),
					       Title = reader["Title"].ToString(),
					       PortalId = reader["PortalId"] == DBNull.Value ? -1 : (int) reader["PortalId"],
					       Email = reader["Email"].ToString(),
					       ImageUrl = reader["ImageUrl"].ToString(),
					       Phone = reader["Phone"].ToString(),
					       Type = (MapObjectType) reader["Type"],
					       BindedObjectIds = ObjectDataHelper.GetObjectList(reader["PoiId"].ToString()).Select(o=>o.Id).ToArray()
				       };
		}

		public static void BindPoiToObject(string poiId, int objectId)
		{
			if (string.IsNullOrEmpty(poiId))
				throw new ArgumentException("Invalid argument", "poiId");

			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_Bind", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };
				cmd.Parameters.AddWithValue("@PoiId", poiId);
				cmd.Parameters.AddWithValue("@ObjectId", objectId);

				sc.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public static void UnbindPoiFromAllObjects(string poiId)
		{
			if (string.IsNullOrEmpty(poiId))
				throw new ArgumentException("Invalid argument", "poiId");

			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_Unbind", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };
				cmd.Parameters.AddWithValue("@PoiId", poiId);

				sc.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public static void UnbindPoiFromObject(string poiId, int objectId)
		{
			if (string.IsNullOrEmpty(poiId))
				throw new ArgumentException("Invalid argument", "poiId");

			using (var sc = new SqlConnection(ConnectionStringHelper.ConnectionString))
			{
				var cmd = new SqlCommand("sp_map_Poi_Unbind", sc)
					          {
						          CommandType = CommandType.StoredProcedure
					          };
				cmd.Parameters.AddWithValue("@PoiId", poiId);
				cmd.Parameters.AddWithValue("@ObjectId", objectId);

				sc.Open();
				cmd.ExecuteNonQuery();
			}
		}
	}
}