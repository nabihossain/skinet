using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL.Repository.Interface
{
    public interface IRepositoryBase<T> where T : class
    {
        IEnumerable<T> FindAll();
        Task<IEnumerable<T>> FindAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindByConditionAsyncDbQuery(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(object id);
        Task DeleteByIdAsync(object id);
        Task BulkInsertOrUpdateAsync(T[] entity);

        Task<IReadOnlyList<T>> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null,
        params Expression<Func<T, object>>[] includes);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        // Methods for executing Store Procedure
        IEnumerable<T> ExecuteSP(string spName, ref SqlParameter[] spParams);
        IEnumerable<T> ExecuteSP(string spName);
        void ExecuteSP(string spName, ref SqlParameter[] spParams, bool isOnlyOutput);
    }
}
