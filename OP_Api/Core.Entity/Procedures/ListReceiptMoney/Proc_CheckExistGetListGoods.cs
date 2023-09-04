using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_CheckExistGetListGoods : IEntityProcView
    {
        public const string ProcName = "Proc_CheckExistGetListGoods";
        [Key]
        public int Id { get; set; }
        public DateTime? FirstLockDate { get; set; }

        public Proc_CheckExistGetListGoods() { }
        public static IEntityProc GetEntityProc(int id)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@Id", id);

            return new EntityProc(
                $"{ProcName} @Id",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
