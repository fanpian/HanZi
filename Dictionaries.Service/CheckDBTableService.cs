using Dictionaries.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries.Service
{
    /// <summary>
    /// 检查并创建表
    /// </summary>
    public static class CheckDBTableService
    {
        public static void Check()
        {
            using (HanZiContext db = new HanZiContext())
            {
                IRelationalDatabaseCreator databaseCreator = db.GetService<IRelationalDatabaseCreator>();
                Console.WriteLine("清空数据库!");
                DeleteTables(db);
                Console.WriteLine("创建数据库!");
                databaseCreator.CreateTables();
            }
        }

        private static void DeleteTables(HanZiContext db)
        {
            string sql = @"
DROP TABLE IF EXISTS ""han_zi"";
DROP TABLE IF EXISTS ""han_zi_ping_yin"";
DROP TABLE IF EXISTS ""ping_yin"";
";
            db.Database.ExecuteSqlRaw(sql);
        }
    }
}
