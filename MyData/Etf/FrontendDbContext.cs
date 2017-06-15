﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using MyData.Migrations;

namespace MyData.Etf
{
    public class FrontendDbContext : DbContext
    {
        public FrontendDbContext() : base("FrontendDbContext")
        {
            Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Models.PostbackData> Postbacks { get; set; }
        public DbSet<Models.RequestLogEntry> RequestLogEntries { get; set; }
        public DbSet<Models.IpSessionId> IpSessionIds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // possibility to set some conventions
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}