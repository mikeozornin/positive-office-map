using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using MapData.Configuration;

namespace MapMvc
{
	public class MapRoleProvider : RoleProvider
	{
		private static List<MemberItem> Managers {
			get
			{
				return MapRolesConfiguration.Current != null
					       ? MapRolesConfiguration.Current.Managers.Cast<MemberItem>().ToList()
					       : null;
			}
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			return RoleExists("Manager") && Managers.Count > 0 && Managers.Any(m => m.Name.Equals(username) && m.IsActive);
		}

		public override string[] GetRolesForUser(string username)
		{
			var userRoles = new List<string>();
			if (IsUserInRole(username, "Manager"))
				userRoles.Add("Manager");
			return userRoles.ToArray();
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			switch (roleName)
			{
				case "Manager":
					return MapRolesConfiguration.Current != null
					       && MapRolesConfiguration.Current.Managers != null;
				default:
					return false;
			}
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string ApplicationName
		{
			get; 
			set;
		}
	}
}
