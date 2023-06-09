using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Core.Entities;
using Project.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Threading.Tasks;

namespace Project.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ProjectDbContext _context;
        public IUserRepository Users { get; }
      

        public UnitOfWork(ProjectDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);

        }

        public IRepository<T> Repository<T>()
            where T : class
        {
            if (typeof(T) == typeof(User))
            {
                return Users as IRepository<T>;
            }
           
            return null;
        }


        public int ExecuteSqlCommand(FormattableString _sql)
        {
            return _context.Database.ExecuteSqlInterpolated(_sql);
        }
        public Task<int> ExecuteSqlCommandAsync(FormattableString _sql)
        {
            return _context.Database.ExecuteSqlInterpolatedAsync(_sql);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public IEnumerable<dynamic> GetObjectsToSQL(string sql)
        {
            var resultSQLRequest = new List<dynamic>();
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {

                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        resultSQLRequest.Add(dataRow);
                    }
                }
            }
            return resultSQLRequest;
        }
        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
