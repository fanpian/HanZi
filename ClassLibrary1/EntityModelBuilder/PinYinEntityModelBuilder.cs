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
    public static class PinYinEntityModelBuilder
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<PinYinEntity> builder = modelBuilder.Entity<PinYinEntity>();
            builder.ToTable("ping_yin");
            builder.Property(p => p.Id).HasColumnName("_id");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("_name");

            builder.HasIndex(p => p.Name).HasDatabaseName("ping_yin_name_index");
        }
    }
}
