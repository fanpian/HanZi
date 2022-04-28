using Dictionaries.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Repository.EntityModelBuilder
{
    public static class HanZiEntityModelBuilder
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<HanZiEntity> builder = modelBuilder.Entity<HanZiEntity>();
            builder.ToTable("han_zi");
            builder.Property(p => p.Id).HasColumnName("_id");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("_name");
            builder.Property(p => p.GB18030).HasColumnName("_gb18030");
            builder.Property(p => p.Unicode).HasColumnName("_unicode");

            builder.HasIndex(p => p.Unicode).HasDatabaseName("han_zi_unicode_index");
            builder.HasIndex(p => p.Name).HasDatabaseName("han_zi_name_index");
        }
    }
}
