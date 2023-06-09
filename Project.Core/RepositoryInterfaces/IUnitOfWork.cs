﻿using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Core.RepositoryInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>()
            where T : class;

        IUserRepository Users { get; }



        IDbContextTransaction BeginTransaction();
        int ExecuteSqlCommand(FormattableString _sql);
        Task<int> ExecuteSqlCommandAsync(FormattableString _sql);
        IEnumerable<dynamic> GetObjectsToSQL(string sql);
        int Complete();
    }
}
