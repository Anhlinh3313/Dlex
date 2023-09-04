using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CreateCustomerInfoLog : IEntityProcView
    {
        public const string ProcName = "Proc_CreateCustomerInfoLog";
        [Key]
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_CreateCustomerInfoLog() { }
        public static IEntityProc GetEntityProc(
            string code,
            string name,
            string phoneNumber,
            string companyName,
            string address,
            string addressNote,
            int? provineId = null,
            int? districtId = null,
            int? wardId = null,
            double? lat = null,
            double? lng = null,
            int? senderId = null)
        {
            SqlParameter Code = new SqlParameter("@Code", code);
            if (string.IsNullOrWhiteSpace(code)) Code.Value = DBNull.Value;

            SqlParameter Name = new SqlParameter("@Name", name);
            if (string.IsNullOrWhiteSpace(name)) Name.Value = DBNull.Value;

            SqlParameter PhoneNumber = new SqlParameter("@PhoneNumber", phoneNumber);
            if (string.IsNullOrWhiteSpace(phoneNumber)) PhoneNumber.Value = DBNull.Value;

            SqlParameter CompanyName = new SqlParameter("@CompanyName", companyName);
            if (string.IsNullOrWhiteSpace(companyName)) CompanyName.Value = DBNull.Value;

            SqlParameter Address = new SqlParameter("@Address", address);
            if (string.IsNullOrWhiteSpace(address)) Address.Value = DBNull.Value;

            SqlParameter AddressNote = new SqlParameter("@AddressNote", addressNote);
            if (string.IsNullOrWhiteSpace(addressNote)) AddressNote.Value = DBNull.Value;

            SqlParameter ProvinceId = new SqlParameter("@ProvinceId", provineId);
            if (!provineId.HasValue) ProvinceId.Value = DBNull.Value;

            SqlParameter DistrictId = new SqlParameter("@DistrictId", districtId);
            if (!districtId.HasValue) DistrictId.Value = DBNull.Value;

            SqlParameter WardId = new SqlParameter("@WardId", wardId);
            if (!wardId.HasValue) WardId.Value = DBNull.Value;

            SqlParameter Lat = new SqlParameter("@Lat", lat);
            if (!lat.HasValue) Lat.Value = DBNull.Value;

            SqlParameter Lng = new SqlParameter("@Lng", lng);
            if (!lng.HasValue) Lng.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue) SenderId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @Code, @Name, @PhoneNumber, @CompanyName, @Address, @AddressNote, @ProvinceId, @DistrictId, @WardId, @Lat, @Lng, @SenderId",
                new SqlParameter[] {
                    Code,Name,PhoneNumber,CompanyName,Address,AddressNote,ProvinceId,DistrictId,WardId,Lat,Lng,SenderId
                }
            );
        }
    }
}
