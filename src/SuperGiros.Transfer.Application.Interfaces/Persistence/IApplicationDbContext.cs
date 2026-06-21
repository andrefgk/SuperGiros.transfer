using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.Interfaces.Persistence
{
    public interface IApplicationDbContext
    {
        //Customer
        DbSet<Customer> customers { get; set; }

        //Transaction
        DbSet<Transaction> transactions { get; set; }
        //Offices
        DbSet<Offices> offices { get; set; }

        DbSet<User> users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
