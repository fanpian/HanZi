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
    public static class HanZiPinYinEntityModelBuilder
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<HanZiPinYinEntity> builder = modelBuilder.Entity<HanZiPinYinEntity>();
            builder.ToTable("han_zi_ping_yin");
            builder.Property(p => p.Id).HasColumnName("_id");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.HanZi).HasColumnName("_han_zi");
            builder.Property(p => p.HanZiId).HasColumnName("_han_zi_id");
            builder.Property(p => p.GB18030).HasColumnName("_gb18030");
            builder.Property(p => p.Unicode).HasColumnName("_unicode");
            builder.Property(p => p.PinYin).HasColumnName("_pin_yin");
            builder.Property(p => p.PinYinId).HasColumnName("_pin_yin_id");
            builder.Property(p => p.YinBiao).HasColumnName("_yin_biao");
            builder.Property(p => p.ShouZiMu).HasColumnName("_shou_zi_mu");
            builder.Property(p => p.BuShou).HasColumnName("_bu_shou");
            builder.Property(p => p.ZhHant).HasColumnName("_zh_hant");
        }
    }
}
