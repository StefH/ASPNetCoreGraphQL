using System;
using System.Linq;
using System.Reflection;
using GraphQL.Types;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Extensions
{
    internal static class TypeExtensions
    {
        public static Type GraphType(this Type type)
        {
            return type.GetTypeInfo().BaseType == typeof(NonNullGraphType) ? type.GetGenericArguments().First() : type;
        }

        public static bool IsObjectGraphType(this Type type)
        {
            return type.GetTypeInfo().BaseType != null && type.GetTypeInfo().BaseType.Name == "ObjectGraphType`1";
        }

        public static Type ModelType(this Type type)
        {
            return type?.GraphType().GetTypeInfo().BaseType?.GetGenericArguments().First();
        }
    }
}