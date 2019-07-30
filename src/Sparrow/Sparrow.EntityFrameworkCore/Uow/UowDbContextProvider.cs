using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Sparrow.EntityFrameworkCore.Uow
{
    public class UowDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {


        public TDbContext GetDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
