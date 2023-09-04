using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetListHubFromUser : IEntityProcView
    {
        public const string ProcName = "Proc_GetListHubFromUser";

        [Key]
        public int Id { get; set; }


        public Proc_GetListHubFromUser()
        {
        }

        public static IEntityProc GetEntityProc(int id)
        {
            return new EntityProc(
                $"{ProcName} @Id",
                new SqlParameter[] {
                new SqlParameter("@Id", id)
                }
            );
        }
    }
}
