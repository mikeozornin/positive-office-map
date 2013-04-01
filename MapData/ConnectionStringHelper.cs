using System.Configuration;
using System.Web.Configuration;

namespace MapData
{
	public static class ConnectionStringHelper
	{
		public static readonly string ConnectionString = string.IsNullOrEmpty(WebConfigurationManager.ConnectionStrings["PTMapConnection"].ConnectionString)
															 ? WebConfigurationManager.ConnectionStrings["PTMapConnection"].ConnectionString
															 : ConfigurationManager.ConnectionStrings["PTMapConnection"].ConnectionString;

		public static readonly string PortalConnectionString = ConfigurationManager.ConnectionStrings["PortalConnection"] == null
															 ? string.Empty
															 : ConfigurationManager.ConnectionStrings["PortalConnection"].ConnectionString;
	}
}