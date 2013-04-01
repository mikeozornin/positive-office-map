using System.Configuration;

namespace MapData.Configuration
{
	public class MapRolesConfiguration : ConfigurationSection
	{
		public static MapRolesConfiguration Current
		{
			get
			{
				MapRolesConfiguration config = (MapRolesConfiguration)ConfigurationManager.GetSection("mapRoles");
				if (config == null)
				{
					throw new ConfigurationErrorsException("MapRolesConfiguration not set.");
				}
				return config;
			}
		}

		[ConfigurationProperty("managers", IsRequired = false)]
		public MemberCollection Managers
		{
			get
			{
				return (MemberCollection)this["managers"];
			}
		}
	}
}
