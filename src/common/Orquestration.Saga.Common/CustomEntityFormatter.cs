using System;
using System.Globalization;
using System.Text.RegularExpressions;
using MassTransit.Topology;

namespace Orquestration.Saga.Common
{
    public class CustomEntityFormatter : IEntityNameFormatter
    {
        private readonly string _sagaName;

        public CustomEntityFormatter(string sagaName) => _sagaName = sagaName;

        public string FormatEntityName<T>() => $"saga:{_sagaName}:{FormatTypeName(typeof(T))}".ToLower(CultureInfo.InvariantCulture);

        private static string FormatTypeName(Type type)
        {
            var typeName = type.Name;

            if (typeName.StartsWith("I")) { typeName = typeName.Substring(1, typeName.Length - 1); }

            var spplited = Regex.Replace(typeName, "([A-Z])", " $1", RegexOptions.Compiled)
                .Trim()
                .Split(' ');

            var entityName = string.Join(".", spplited);

            return entityName;
        }
    }
}