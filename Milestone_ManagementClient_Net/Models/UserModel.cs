using System;
using System.Collections.Generic;

namespace MilestoneManagementClient.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public string AuthType { get; set; } // "Basic User" or "Windows User"
        public bool IsEnabled { get; set; }
        public DateTime LastLogin { get; set; }

        public UserModel()
        {
            Username = "";
            DisplayName = "";
            Role = "Operators";
            AuthType = "Basic User";
            IsEnabled = true;
            LastLogin = DateTime.Now.AddHours(-2);
        }

        public UserModel(string username, string displayName, string role, string authType)
        {
            Username = username;
            DisplayName = displayName;
            Role = role;
            AuthType = authType;
            IsEnabled = true;
            LastLogin = DateTime.Now.AddHours(-1);
        }
    }

    public class RoleModel
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<string> Permissions { get; set; }

        public RoleModel()
        {
            RoleName = "";
            Description = "";
            Permissions = new List<string>();
        }

        public RoleModel(string name, string desc, string[] perms)
        {
            RoleName = name;
            Description = desc;
            Permissions = new List<string>(perms);
        }
    }
}
