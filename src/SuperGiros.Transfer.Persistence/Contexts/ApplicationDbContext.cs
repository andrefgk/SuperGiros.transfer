using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Persistence.Interceptors;
using System.Reflection;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangeInterceptor;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangeInterceptor) : base(options)
        {
            _auditableEntitySaveChangeInterceptor = auditableEntitySaveChangeInterceptor;
        }

        //Customer
        public DbSet<Customer> customer { get; set; }

        // 2. Estas son solo para cumplir con la Interfaz (EF las ignora porque no tienen set automático)
        public DbSet<Customer> customers { get => customer; set => customer = value; }

        DbSet<Customer> IApplicationDbContext.customers
        {
            get => customer;
            set => customer = value;
        }


        //public DbSet<Customer> customer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //Transaction
        public DbSet<Transaction> transaction { get; set; }
        public DbSet<Transaction> transactions { get => transaction; set => transaction = value; }
        DbSet<Transaction> IApplicationDbContext.transactions
        {
            get => transaction;
            set => transaction = value;
        }

        //Offices
        public DbSet<Offices> office { get; set; }
        public DbSet<Offices> offices { get; set; }

        //User
        public DbSet<User> users { get; set; }

        // Si necesitas que 'customers' (en plural) apunte a 'customer' (en singular)
        // hazlo de forma que no llame al setter de sí mismo:
        DbSet<Offices> IApplicationDbContext.offices
        {
            get => office;
            set => office = value;
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangeInterceptor);
            optionsBuilder.EnableSensitiveDataLogging();

           
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            return await base.SaveChangesAsync(cancellationToken);

        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            // Redirigimos al método real de EF Core
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
