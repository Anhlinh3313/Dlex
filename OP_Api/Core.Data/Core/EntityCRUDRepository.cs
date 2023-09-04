using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Data.Abstract;
using Core.Entity.Abstract;
using Core.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Core.Infrastructure.Http;
using Core.Entity.Procedures;
using System.Reflection;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;

namespace Core.Data.Core
{
    public class EntityCRUDRepository<T> : EntityRRepository<T>, IEntityCRUDRepository<T>
        where T : class, IEntityBase, new()
    {
        private ApplicationContext _context;
        private string _tableName;
        private int _userId;
        private int _companyId;
        private string _userGent;
        private int _typeUserAgent;

        public EntityCRUDRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            var mapping = _context.Model.FindEntityType(typeof(T)).Relational();
            _tableName = mapping.TableName;
            if (HttpContext.Current != null)
            {
                _userId = HttpContext.CurrentUserId;
                _userGent = HttpContext.GetRequestHeaders(TypeHTTPRequestHeader.UserAgent);
                _typeUserAgent = UserAgentUtil.GetTypeUserAgentHeader(_userGent);
                _companyId = HttpContext.CurrentCompanyId;
            }
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            //dbEntityEntry.State = EntityState.Deleted;
            //
            var proc = Proc_Core_DeleteTable.GetEntityProc(_tableName, entity.Id, _userId);
            var tableResult = _context.Set<Proc_Core_DeleteTable>().FromSql(proc.GetQuery(), proc.GetParams());
            var data = tableResult.FirstOrDefault() as Proc_Core_DeleteTable;
        }

        public void DeleteEmpty(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public void Delete(int id)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(f50P_CR367 => f50P_CR367.Id == id && f50P_CR367.CompanyId == _companyId);

            foreach (var entity in entities)
            {
                //_context.Entry<T>(entity).State = EntityState.Deleted;
                //
                var proc = Proc_Core_DeleteTable.GetEntityProc(_tableName, entity.Id, _userId);
                var tableResult = _context.Set<Proc_Core_DeleteTable>().FromSql(proc.GetQuery(), proc.GetParams());
                var data = tableResult.FirstOrDefault() as Proc_Core_DeleteTable;
            }
        }

        public void DeleteEmpty(int id)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(f50P_CR81 => f50P_CR81.Id == id);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                //_context.Entry<T>(entity).State = EntityState.Deleted; 
                //
                var proc = Proc_Core_DeleteTable.GetEntityProc(_tableName, entity.Id, _userId);
                var tableResult = _context.Set<Proc_Core_DeleteTable>().FromSql(proc.GetQuery(), proc.GetParams());
                var data = tableResult.FirstOrDefault() as Proc_Core_DeleteTable;
            }

        }

        public void DeleteEmptyWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }

        }

        public void Insert(T entity)
        {
            if (entity is IEntityBasic)
            {
                DateTime currentDate = DateTime.Now;
                int currentUserId = HttpContext.CurrentUserId;
                var tempEntity = entity as IEntityBasic;
                tempEntity.CreatedBy = currentUserId;
                tempEntity.CreatedWhen = currentDate;
                tempEntity.ModifiedBy = currentUserId;
                tempEntity.ModifiedWhen = currentDate;
                tempEntity.ConcurrencyStamp = Guid.NewGuid().ToString();
                // get http header
                PropertyInfo createdByUAType = tempEntity.GetType().GetProperty("CreatedByUAType");
                if (createdByUAType != null)
                {
                    createdByUAType.SetValue(tempEntity, _typeUserAgent);
                }

                entity = tempEntity as T;
            }
            entity.IsEnabled = true;
            entity.CompanyId = _companyId;
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public void InsertNoneUser(T entity)
        {
            if (entity is IEntityBasic)
            {
                DateTime currentDate = DateTime.Now;
                var tempEntity = entity as IEntityBasic;
                tempEntity.CreatedWhen = currentDate;
                tempEntity.ModifiedWhen = currentDate;
                tempEntity.ConcurrencyStamp = Guid.NewGuid().ToString();
                // get http header
                PropertyInfo createdByUAType = tempEntity.GetType().GetProperty("CreatedByUAType");
                if (createdByUAType != null)
                {
                    createdByUAType.SetValue(tempEntity, _typeUserAgent);
                }

                entity = tempEntity as T;
            }
            entity.IsEnabled = true;
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public void InsertMDF(T entity)
        {
            if (entity is IEntityBasic)
            {
                DateTime currentDate = DateTime.Now;
                int currentUserId = HttpContext.CurrentUserId;
                var tempEntity = entity as IEntityBasic;
                tempEntity.CreatedBy = currentUserId;
                tempEntity.ModifiedBy = currentUserId;
                tempEntity.ConcurrencyStamp = Guid.NewGuid().ToString();
                // get http header
                PropertyInfo createdByUAType = tempEntity.GetType().GetProperty("CreatedByUAType");
                if (createdByUAType != null)
                {
                    createdByUAType.SetValue(tempEntity, _typeUserAgent);
                }

                entity = tempEntity as T;
            }
            entity.IsEnabled = true;
            entity.CompanyId = _companyId;
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public void InsertAndUpdate(T entity)
        {
            if (entity.Id == 0)
                Insert(entity);
            else
                Update(entity);
        }

        public void Update(T entity)
        {
            if (entity is IEntityBasic)
            {
                DateTime currentDate = DateTime.Now;
                int currentUserId = HttpContext.CurrentUserId;
                var tempEntity = entity as IEntityBasic;
                tempEntity.ModifiedBy = currentUserId;
                tempEntity.ModifiedWhen = currentDate;
                tempEntity.ConcurrencyStamp = Guid.NewGuid().ToString();
                tempEntity.CompanyId = _companyId;

                // get http header
                PropertyInfo modifiedByUAType = tempEntity.GetType().GetProperty("ModifiedByUAType");
                if (modifiedByUAType != null)
                {
                    modifiedByUAType.SetValue(tempEntity, _typeUserAgent);
                }
                entity = tempEntity as T;
            }
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

    }
}
