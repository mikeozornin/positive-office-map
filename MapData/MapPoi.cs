namespace MapData
{
	public class MapPoi
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public int? PortalId { get; set; }
		public string Email { get; set; }
		public string ImageUrl { get; set; }
		public string Phone { get; set; }
		public MapObjectType Type { get; set; }
		public int[] BindedObjectIds { get; set; }
	}
}