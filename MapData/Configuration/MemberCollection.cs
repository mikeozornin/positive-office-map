using System.Collections.Generic;
using System.Configuration;

namespace MapData.Configuration
{
	public class MemberCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new MemberItem();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MemberItem)(element)).Name;
		}

		public MemberItem this[int index]
		{
			get
			{
				return (MemberItem)BaseGet(index);
			}
		}
	}
}
