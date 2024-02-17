using AzFunctionCleanTemplate.Application.Interfaces;
using AzFunctionCleanTemplate.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzFunctionCleanTemplate.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly Lazy<DataverseContext> _context;
        private readonly ILogger<Repository<T>> _logger;
        private readonly string _entityLogicalName;

        public Repository(Lazy<DataverseContext> context,
            string entityLogicalName,
            ILogger<Repository<T>> logger)
        {
            _context = context;
            _entityLogicalName = DataverseMapper.GetEntityLogicalName<T>();
            _logger = logger;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken token)
        {
            // Retrieve the entity using the logical name and map it to the domain model
            try
            {
                Entity entity = await _context.Value.ServiceClient.RetrieveAsync(_entityLogicalName, id, new ColumnSet(true), token);
                if (entity != null)
                {
                    return DataverseMapper.ToDomainModel<T>(entity);
                }
            }
            catch (Exception ex)
            {
                //Due to sdk return error if the Id doesn't exist
                _logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task CreateAsync(T entity)
        {
            Entity dataverseEntity = DataverseMapper.ToDataverseEntity(entity);
            await _context.Value.ServiceClient.CreateAsync(dataverseEntity);
            // Set the ID back to the domain model if necessary
            PropertyInfo idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.CanWrite)
            {
                idProperty.SetValue(entity, dataverseEntity.Id);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            Entity dataverseEntity = DataverseMapper.ToDataverseEntity(entity);
            await _context.Value.ServiceClient.UpdateAsync(dataverseEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            // Delete the entity using the logical name and the ID
            await _context.Value.ServiceClient.DeleteAsync(_entityLogicalName, id);
        }
    }
}
