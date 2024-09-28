﻿using Microsoft.EntityFrameworkCore;

using System.Data;
using Configuration;
using DbModels;
using System;
using Microsoft.Identity.Client;
using Models;
using Microsoft.EntityFrameworkCore.Migrations;
//using Models.DTO;
using Microsoft.Extensions.Options;

namespace DbContext;

//DbContext namespace is a fundamental EFC layer of the database context and is
//used for all Database connection as well as for EFC CodeFirst migration and database updates 

public class csMainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    static public string Heartbeat { get; } = $"Heartbeat from namespace {nameof(DbContextOptions)}, class {nameof(csMainDbContext)}";

    #region C# model of database tables
    public DbSet<csUserDbM> Users { get; set; }
    public DbSet<csReviewDbM> Reviews { get; set; }
    public DbSet<csAttractionDbM> Attractions { get; set; }
    public DbSet<csLocationDbM> Locations { get; set; }
    #endregion


    #region get right context from DbSet configuration in json file and UserLogin
    public static DbContextOptionsBuilder<csMainDbContext> DbContextOptions(DbLoginDetail loginDetail)
    {
        var _optionsBuilder = new DbContextOptionsBuilder<csMainDbContext>();

        if (loginDetail.DbServer == "SQLServer")
        {
            _optionsBuilder.UseSqlServer(loginDetail.DbConnectionString,
                    options => options.EnableRetryOnFailure());
            return _optionsBuilder;
        }
        else if (loginDetail.DbServer == "MariaDb")
        {
            _optionsBuilder.UseMySql(loginDetail.DbConnectionString, ServerVersion.AutoDetect(loginDetail.DbConnectionString));
            return _optionsBuilder;
        }
        else if (loginDetail.DbServer == "Postgres")
        {
            _optionsBuilder.UseNpgsql(loginDetail.DbConnectionString);
            return _optionsBuilder;
        }
        else if (loginDetail.DbServer == "SQLite")
        {
            _optionsBuilder.UseSqlite(loginDetail.DbConnectionString);
            return _optionsBuilder;
        }

        //unknown database type
        throw new InvalidDataException($"Database type {loginDetail.DbServer} does not exist");
    }

    //Given a userlogin, this method finds the LoginDetails in the Active DbSet and return a DbContext
    public static csMainDbContext DbContext(string DbUserLogin) =>
        new csMainDbContext(csMainDbContext.DbContextOptions(csAppConfig.DbLoginDetails(DbUserLogin)).Options);

    #endregion

    #region constructors
    public csMainDbContext() { }
    public csMainDbContext(DbContextOptions options) : base(options)
    { }
    #endregion

    #region model the Views
    #endregion

    //Here we can modify the migration building
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region model the Views
        #endregion

        #region override modelbuilder

        // Define unique index for City and Country in csLocationDbM
        /* //SHOULD BE A ANNOTATION?
        modelBuilder.Entity<csLocationDbM>()
            .HasIndex(l => new { l.City, l.Country })
            .IsUnique();

        */

        modelBuilder.Entity("DbModels.csReviewDbM", b =>
                {
                    b.HasOne("DbModels.csAttractionDbM", "AttractionDbM")
                        .WithMany("ReviewsDbM")
                        .HasForeignKey("AttractionDbMAttractionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DbModels.csUserDbM", "UserDbM")
                        .WithMany("ReviewsDbM")
                        .HasForeignKey("UserDbMUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("AttractionDbM");

                    b.Navigation("UserDbM");
                });

        /*modelBuilder.Entity("DbModels.csAttractionDbM", b =>
        {
            b.HasOne("DbModels.csLocationDbM", "LocationDbM")
                .WithMany("AttractionsDbM")
                .HasForeignKey("LocationId")
                .OnDelete(DeleteBehavior.SetNull);

            b.Navigation("LocationDbM");
        }); */


        #endregion

        base.OnModelCreating(modelBuilder);
    }

    #region DbContext for some popular databases
    public class SqlServerDbContext : csMainDbContext
    {
        public SqlServerDbContext() { }
        public SqlServerDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration and database update commands
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = csAppConfig.DbSetActive.DbLogins.Find(
                    i => i.DbServer == "SQLServer" && i.DbUserLogin == "sysadmin").DbConnectionString;
                optionsBuilder.UseSqlServer(connectionString,
                    options => options.EnableRetryOnFailure());

            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("nvarchar(200)");

            base.ConfigureConventions(configurationBuilder);
        }

        #region Add your own modelling based on done migrations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        #endregion

    }

    public class MySqlDbContext : csMainDbContext
    {
        public MySqlDbContext() { }
        public MySqlDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = csAppConfig.DbSetActive.DbLogins.Find(
                    i => i.DbServer == "MariaDb" && i.DbUserLogin == "sysadmin").DbConnectionString;
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);

        }
    }

    public class PostgresDbContext : csMainDbContext
    {
        public PostgresDbContext() { }
        public PostgresDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = csAppConfig.DbSetActive.DbLogins.Find(
                    i => i.DbServer == "Postgres" && i.DbUserLogin == "sysadmin").DbConnectionString;
                optionsBuilder.UseNpgsql(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");
            base.ConfigureConventions(configurationBuilder);
        }
    }

    public class SqliteDbContext : csMainDbContext
    {
        public SqliteDbContext() { }
        public SqliteDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = csAppConfig.DbSetActive.DbLogins.Find(
                    i => i.DbServer == "SQLite" && i.DbUserLogin == "sysadmin").DbConnectionString;
                optionsBuilder.UseSqlite(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
    #endregion
}


