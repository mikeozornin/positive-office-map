using System.Configuration;

namespace MapData.Configuration
{
	public class MemberItem : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}
		}

		[ConfigurationProperty("isActive", IsRequired = true)]
		public bool IsActive
		{
			get
			{
				return (bool)this["isActive"];
			}
		}
	}
}
