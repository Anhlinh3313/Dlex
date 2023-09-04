using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateAreaDistricts : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateAreaDistricts";

        [Key]
        public bool IsDelete { get; set; }

        public Proc_UpdateAreaDistricts()
        {
        }

        public static IEntityProc GetEntityProc(int areaId, int[] districtIds, int[] fromProvinceIds)
        {
            SqlParameter parameter1 = new SqlParameter(
            "@AreaId", areaId);
            //
            SqlParameter parameter2 = new SqlParameter(
            "@listDistrictString", "");
            if (districtIds.Length>0)
                parameter2.Value = string.Join(",",districtIds);
            //
            SqlParameter parameter3 = new SqlParameter(
            "@listProvinceString", "");
            if (fromProvinceIds.Length > 0)
                parameter3.Value = string.Join(",", fromProvinceIds);
            //
            return new EntityProc(
                $"{ProcName} @AreaId, @listDistrictString, @listProvinceString",
                new SqlParameter[] {
                    parameter1,
                    parameter2,
                    parameter3
                }
            );
        }
    }
}
