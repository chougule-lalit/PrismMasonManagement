using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api.Authorization
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
        }
        public static class Items
        {
            public const string View = "Permissions.Items.View";
            public const string Create = "Permissions.Items.Create";
            public const string Edit = "Permissions.Items.Edit";
            public const string Delete = "Permissions.Items.Delete";
        }
    }
}
