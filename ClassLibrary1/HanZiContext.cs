using Dictionaries.Model.Entities;
using Dictionaries.Repository.EntityModelBuilder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Repository
{
    /// <summary>
    /// DbContext
    /// </summary>
    public class HanZiContext : DbContext
    {
        private static readonly string dbFile = Path.Combine(@"D:\fanpian\privateJob\Dictionaries\Dictionaries", "App_Data", "dictionaries.sqlite3");
        public HanZiContext()
            : this(dbFile)
        { }

        public HanZiContext(string filePath)
            : this(getOptions(filePath))
        {
        }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public HanZiContext(DbContextOptions<HanZiContext> options)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            : base(options)
        {

        }
        
        /// <summary>
        /// 根据路径生成DbContextOptions
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static DbContextOptions<HanZiContext> getOptions(string filePath)
        {
            return new DbContextOptionsBuilder<HanZiContext>()
                                                        .UseSqlite($"Data Source={filePath}")
                                                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                        .Options;
        }

        /// <summary>
        /// 汉字集合
        /// </summary>
        public DbSet<HanZiEntity> HanZiEntities { get; set; }

        public DbSet<PinYinEntity> PinYinEntities { get; set; }

        public DbSet<HanZiPinYinEntity> HanZiPinYinEntities { get; set; }
        
        /// <summary>
        /// 模型对象属性与数据库字段绑定
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            HanZiEntityModelBuilder.OnModelCreating(modelBuilder);
            PinYinEntityModelBuilder.OnModelCreating(modelBuilder);
            HanZiPinYinEntityModelBuilder.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
