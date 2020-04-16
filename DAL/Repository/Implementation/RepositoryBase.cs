using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Repository.Interface;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repository.Implementation
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _SpecificEntity;
        public RepositoryBase(DbContext context)
        {
            _context = context;
            _SpecificEntity = _context.Set<T>();
        }

        public async Task BulkInsertOrUpdateAsync(T[] entity)
        {
            await _context.BulkInsertOrUpdateAsync(entity);
        }

        public void Create(T entity)
        {
            _SpecificEntity.Add(entity);
        }

        public void Delete(T entity)
        {
            _SpecificEntity.Remove(entity);
        }

        public void DeleteById(object id)
        {
            T entity = _SpecificEntity.Find(id);
            _SpecificEntity.Remove(entity);
        }

        public async Task DeleteByIdAsync(object id)
        {
            T entity = await _SpecificEntity.FindAsync(id);
            _SpecificEntity.Remove(entity);
        }

        public IEnumerable<T> ExecuteSP(string spName, ref SqlParameter[] spParams)
        {
            string sqlCommand = "";
            sqlCommand = sqlCommand + "exec " + spName + " ";
            for (int i = 0; i < spParams.Length; i++)
            {
                sqlCommand = sqlCommand + "@" + spParams[i].ParameterName + " ";
                if (spParams[i].Direction == System.Data.ParameterDirection.Output)
                {
                    sqlCommand = sqlCommand + "out ";
                }
                if (i != (spParams.Length - 1))
                {
                    sqlCommand = sqlCommand + ", ";
                }
            }
            IEnumerable<T> t = _SpecificEntity.FromSqlRaw("" + sqlCommand, spParams);
            return t;
        }
        public IEnumerable<T> ExecuteSP(string spName)
        {
            string sqlCommand = "";
            sqlCommand = sqlCommand + "exec " + spName + " ";
            IEnumerable<T> t = _SpecificEntity.FromSqlRaw("" + sqlCommand);
            return t;
        }
        public void ExecuteSP(string spName, ref SqlParameter[] spParams, bool isOnlyOutput)
        {
            string sqlCommand = "";
            sqlCommand = sqlCommand + "exec " + spName + " ";
            for (int i = 0; i < spParams.Length; i++)
            {
                sqlCommand = sqlCommand + "@" + spParams[i].ParameterName + " ";
                if (spParams[i].Direction == System.Data.ParameterDirection.Output)
                {
                    sqlCommand = sqlCommand + "out ";
                }

                if (i != (spParams.Length - 1))
                {
                    sqlCommand = sqlCommand + ", ";
                }
            }
            int rowcount = _context.Database.ExecuteSqlRaw("" + sqlCommand, spParams);
        }

        public IEnumerable<T> FindAll()
        {
            return _SpecificEntity;
        }
        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _SpecificEntity.ToListAsync();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _SpecificEntity.Where(expression);
        }
        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _SpecificEntity.Where(expression).ToListAsync();
        }

        public Task<IEnumerable<T>> FindByConditionAsyncDbQuery(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
                                   IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null,
                                   params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _SpecificEntity;

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _SpecificEntity.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _SpecificEntity.Where(expression).ToListAsync(); ;
        }

        public void Update(T entity)
        {
            _SpecificEntity.Update(entity);
        }
    }
}