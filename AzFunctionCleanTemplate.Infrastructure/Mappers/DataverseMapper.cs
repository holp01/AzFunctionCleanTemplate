using AzFunctionCleanTemplate.Domain.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionCleanTemplate.Infrastructure.Mappers
{
    public static class DataverseMapper
    {
        public static Entity ToDataverseEntity<T>(T model) where T : class
        {
            var entityName = GetEntityLogicalName<T>();
            var entity = new Entity(entityName);

            foreach (var prop in typeof(T).GetProperties())
            {
                var logicalName = prop.GetCustomAttribute<LogicalNameAttribute>()?.Name;
                var optionSet = prop.GetCustomAttribute<DataverseOptionSetAttribute>()?.OptionSetName;
                var foreignKey = prop.GetCustomAttribute<DataverseForeignKeyAttribute>()?.ForeignKey;

                var value = prop.GetValue(model);
                if (value == null) continue;

                if (!string.IsNullOrEmpty(logicalName))
                {
                    entity[logicalName] = value;
                }
                else if (!string.IsNullOrEmpty(optionSet) && value is int intValue)
                {
                    entity[optionSet] = new OptionSetValue(intValue);
                }
                else if (!string.IsNullOrEmpty(foreignKey) && value is Guid guidValue)
                {
                    entity[foreignKey] = new EntityReference(foreignKey, guidValue);
                }
            }

            return entity;
        }

        public static string GetEntityLogicalName<T>()
        {
            var entityType = typeof(T);
            var entityAttribute = entityType.GetCustomAttribute<DataverseTableNameAttribute>();
            if (entityAttribute != null)
            {
                return entityAttribute.TableName;
            }

            // Fallback to the type name if no attribute is found
            return entityType.Name.ToLower();
        }

        public static T ToDomainModel<T>(Entity entity) where T : class, new()
        {
            var model = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                var logicalName = prop.GetCustomAttribute<LogicalNameAttribute>()?.Name;
                var optionSet = prop.GetCustomAttribute<DataverseOptionSetAttribute>()?.OptionSetName;
                var foreignKey = prop.GetCustomAttribute<DataverseForeignKeyAttribute>()?.ForeignKey;

                if (!string.IsNullOrEmpty(logicalName) && entity.Contains(logicalName))
                {
                    var value = entity[logicalName];
                    prop.SetValue(model, value);
                }
                else if (!string.IsNullOrEmpty(optionSet) && entity.Contains(optionSet))
                {
                    var optionSetValue = entity[optionSet] as OptionSetValue;
                    prop.SetValue(model, optionSetValue?.Value);
                }
                else if (!string.IsNullOrEmpty(foreignKey) && entity.Contains(foreignKey))
                {
                    var entityReference = entity[foreignKey] as EntityReference;
                    prop.SetValue(model, entityReference?.Id);
                }
            }
            return model;
        }
    }
}
