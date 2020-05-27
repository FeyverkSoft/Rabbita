using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Rabbita.Helpers
{
    internal static class TypeHelper
    {
        /// <summary>
        /// Возвращает список генериков для типа обработчика
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="genericHandler"></param>
        /// <returns></returns>
        internal static List<Type> GetGenericInterfaces(this Type handler, Type genericHandler)
        {
            return handler.GetTypeInfo()
                .ImplementedInterfaces
                .Select(iface => iface.GetTypeInfo())
                .Where(typeInfo => typeInfo.IsInterface && typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GenericTypeArguments[0])
                .ToList();
        }
    }
}