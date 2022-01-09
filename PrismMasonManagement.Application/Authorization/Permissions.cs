using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Authorization
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

        public static List<Type> GetAllPermissionTypes()
        {
            return new List<Type>
                    {
                        typeof(Items),
                        typeof(Products)
                    };
        }

        public static class Items
        {
            public const string View = "Permissions.Items.View";
            public const string Create = "Permissions.Items.Create";
            public const string Edit = "Permissions.Items.Edit";
            public const string Delete = "Permissions.Items.Delete";
        }

        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }
    }
}
