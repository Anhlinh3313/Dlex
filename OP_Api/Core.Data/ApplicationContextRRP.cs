﻿using System;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class ApplicationContextRRP : DbContext
    {
        //public ApplicationContext(DbContextOptions options) : base(options) { }

        public ApplicationContextRRP(DbContextOptions<ApplicationContextRRP> options) : base(options) { }

        public ApplicationContextRRP()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.UseSqlServer(ConnectionRRP.Instance.GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //General
            //modelBuilder.Entity<User>().ToTable("Core_User");
            //modelBuilder.Entity<UserRelation>().ToTable("Core_UserRelation");
            //modelBuilder.Entity<UserRole>().ToTable("Core_UserRole");
            //modelBuilder.Entity<Role>().ToTable("Core_Role");
            //modelBuilder.Entity<Department>().ToTable("Core_Department");
            //modelBuilder.Entity<Country>().ToTable("Core_Country");
            //modelBuilder.Entity<Province>().ToTable("Core_Province");
            //modelBuilder.Entity<District>().ToTable("Core_District");
            //modelBuilder.Entity<Ward>().ToTable("Core_Ward");
            //modelBuilder.Entity<Hub>().ToTable("Core_Hub");
            //modelBuilder.Entity<HubRoute>().ToTable("Core_HubRoute");
            //modelBuilder.Entity<HubRoutingWard>().ToTable("Core_HubRoutingWard");
            //modelBuilder.Entity<Street>().ToTable("Core_Street");
            //modelBuilder.Entity<StreetJoin>().ToTable("Core_StreetJoin");
            //modelBuilder.Entity<HubRoutingStreetJoin>().ToTable("Core_HubRoutingStreetJoin");
            ////CRM
            //modelBuilder.Entity<Customer>().ToTable("Crm_Customer");
            //modelBuilder.Entity<CusDepartment>().ToTable("Crm_CusDepartment");
            //modelBuilder.Entity<CustomerSettinng>().ToTable("Crm_CustomerSetting");
            ////Post
            //modelBuilder.Entity<Shipment>().ToTable("Post_Shipment");
            //modelBuilder.Entity<LadingSchedule>().ToTable("Post_LadingSchedule");
            //modelBuilder.Entity<RequestShipment>().ToTable("Post_RequestShipment");
            //modelBuilder.Entity<RequestLadingSchedule>().ToTable("Post_RequestLadingSchedule");
            //modelBuilder.Entity<ShipmentStatus>().ToTable("Post_ShipmentStatus");
            //modelBuilder.Entity<Reason>().ToTable("Post_Reason");
            //modelBuilder.Entity<AreaGroup>().ToTable("Post_AreaGroup");
            //modelBuilder.Entity<Area>().ToTable("Post_Area");
            //modelBuilder.Entity<AreaDistrict>().ToTable("Post_AreaDistrict");
            //modelBuilder.Entity<Formula>().ToTable("Post_Formula");
            //modelBuilder.Entity<WeightGroup>().ToTable("Post_WeightGroup");
            //modelBuilder.Entity<Weight>().ToTable("Post_Weight");
            //modelBuilder.Entity<PriceList>().ToTable("Post_PriceList");
            //modelBuilder.Entity<PriceService>().ToTable("Post_PriceService");
            //modelBuilder.Entity<PriceServiceDetail>().ToTable("Post_PriceServiceDetail");
            //modelBuilder.Entity<CustomerPriceList>().ToTable("Post_CustomerPriceList");
            //modelBuilder.Entity<Service>().ToTable("Post_Service");
            //modelBuilder.Entity<ServiceDVGT>().ToTable("Post_ServiceDVGT");
            //modelBuilder.Entity<PackType>().ToTable("Post_PackType");
            //modelBuilder.Entity<PaymentType>().ToTable("Post_PaymentType");
            //modelBuilder.Entity<Structure>().ToTable("Post_Structure");
            //modelBuilder.Entity<ShipmentServiceDVGT>().ToTable("Post_ShipmentServiceDVGT");
            //modelBuilder.Entity<RequestShipmentServiceDVGT>().ToTable("Post_RequestShipmentServiceDVGT");
            //modelBuilder.Entity<ServiceDVGTPrice>().ToTable("Post_ServiceDVGTPrice");
            //modelBuilder.Entity<Box>().ToTable("Post_Box");
            //modelBuilder.Entity<Package>().ToTable("Post_Package");
            //modelBuilder.Entity<ShipmentPackage>().ToTable("Post_ShipmentPackage");
            //modelBuilder.Entity<Size>().ToTable("Post_Size");
            //modelBuilder.Entity<ChargedCOD>().ToTable("Post_ChargedCOD");
            //modelBuilder.Entity<ChargedRemote>().ToTable("Post_ChargedRemote");
            //modelBuilder.Entity<ListGoods>().ToTable("Post_ListGoods");
            //modelBuilder.Entity<ListGoodsType>().ToTable("Post_ListGoodsType");
            //modelBuilder.Entity<ShipmentListGoods>().ToTable("Post_ShipmentListGoods");
            //modelBuilder.Entity<ListReceiptMoney>().ToTable("Post_ListReceiptMoney");
            //modelBuilder.Entity<ListReceiptMoneySchedule>().ToTable("Post_ListReceiptMoneySchedule");
            //modelBuilder.Entity<ListReceiptMoneyShipment>().ToTable("Post_ListReceiptMoneyShipment");
            //modelBuilder.Entity<ListReceiptConfirmMoney>().ToTable("Post_ListReceiptConfirmMoney");
            //modelBuilder.Entity<ListReceiptConfirmMoneyShipment>().ToTable("Post_ListReceiptConfirmMoneyShipment");
            //modelBuilder.Entity<ListReceiptMoneyStatus>().ToTable("Post_ListReceiptMoneyStatus");
            //modelBuilder.Entity<ListReceiptMoneyType>().ToTable("Post_ListReceiptMoneyType");
            //modelBuilder.Entity<ShipmentVersion>().ToTable("Post_ShipmentVersion");
            //modelBuilder.Entity<ShipmentServiceDVGTVersion>().ToTable("Post_ShipmentServiceDVGTVersion");
            //modelBuilder.Entity<ListCustomerPayment>().ToTable("Post_ListCustomerPayment");
            //modelBuilder.Entity<ListCustomerPaymentType>().ToTable("Post_ListCustomerPaymentType");
            //modelBuilder.Entity<ListCustomerPaymentShipment>().ToTable("Post_ListCustomerPaymentShipment");
            //modelBuilder.Entity<ListCustomerPaymentSchedule>().ToTable("Post_ListCustomerPaymentSchedule");
            //modelBuilder.Entity<TPL>().ToTable("Post_TPL");
            //modelBuilder.Entity<TPLTransit>().ToTable("Post_TPLTransit");
            //modelBuilder.Entity<ListGoodsStatus>().ToTable("Post_ListGoodsStatus");
            //modelBuilder.Entity<ProvideCodeStatus>().ToTable("Post_ProvideCodeStatus");
            //modelBuilder.Entity<ProvideCode>().ToTable("Post_ProvideCode");
            //modelBuilder.Entity<DeadlinePickupDelivery>().ToTable("Post_DeadlinePickupDelivery");
            //modelBuilder.Entity<DeadlinePickupDeliveryDetail>().ToTable("Post_DeadlinePickupDeliveryDetail");
            //modelBuilder.Entity<TransportType>().ToTable("Post_TransportType");
            //modelBuilder.Entity<TPLTransportType>().ToTable("Post_TPLTransportType");
            //modelBuilder.Entity<ShipmentImage>().ToTable("Post_ShipmentImage");
            //modelBuilder.Entity<PriceListDVGT>().ToTable("Post_PriceListDVGT");
            //modelBuilder.Entity<PackagePrice>().ToTable("Post_PackagePrice");
            //modelBuilder.Entity<CalculateBy>().ToTable("Post_CalculateBy");
            //modelBuilder.Entity<TelephoneAreaCodes>().ToTable("Post_TelephoneAreaCodes");
            //modelBuilder.Entity<PriceListDVGT>().ToTable("Post_PriceListDVGT");
            //modelBuilder.Entity<PackagePrice>().ToTable("Post_PackagePrice");
            //modelBuilder.Entity<CalculateBy>().ToTable("Post_CalculateBy");
            //modelBuilder.Entity<UserPrintShipment>().ToTable("Post_ListUserPrintShipment");
            //modelBuilder.Entity<StatusPrintShipment>().ToTable("Post_StatusPrintShipment");
            //modelBuilder.Entity<ShipmentPrintType>().ToTable("Post_ShipmentPrintType");
            //modelBuilder.Entity<TruckSchedule>().ToTable("Post_TruckSchedule");
            //modelBuilder.Entity<TruckScheduleStatus>().ToTable("Post_TruckScheduleStatus");
            //modelBuilder.Entity<TruckScheduleRider>().ToTable("Post_TruckScheduleRider");
            //modelBuilder.Entity<Truck>().ToTable("Core_Truck");
            //modelBuilder.Entity<TruckScheduleDetail>().ToTable("Post_TruckScheduleDetail");
            //modelBuilder.Entity<ShipmentType>().ToTable("Post_ShipmentType");
            //modelBuilder.Entity<TruckScheduleImage>().ToTable("Post_TruckScheduleImage");
            //modelBuilder.Entity<UploadExcelHistory>().ToTable("Post_UploadExcelHistory");
            //modelBuilder.Entity<FromProvinceService>().ToTable("Post_FromProvinceService");
            //modelBuilder.Entity<RemotePrice>().ToTable("Post_RemotePrice");
            //modelBuilder.Entity<RemoteKm>().ToTable("Post_RemoteKm");
            //modelBuilder.Entity<RemotePriceDetail>().ToTable("Post_RemotePriceDetail");
            //modelBuilder.Entity<FromProvinceArea>().ToTable("Post_FromProvinceArea");
            //modelBuilder.Entity<CustomerPriceService>().ToTable("Post_CustomerPriceService");
            //modelBuilder.Entity<PricingType>().ToTable("Post_PricingType");
            //modelBuilder.Entity<TaskScheduler>().ToTable("Post_TaskScheduler");
            //modelBuilder.Entity<CustomerPriceListDVGT>().ToTable("Post_CustomerPriceListDVGT");
            //modelBuilder.Entity<PaymentCODType>().ToTable("Post_PaymentCODType");
            //modelBuilder.Entity<FormPrintType>().ToTable("Post_FormPrintType");
            //modelBuilder.Entity<FormPrint>().ToTable("Post_FormPrint");
            //modelBuilder.Entity<Holiday>().ToTable("Post_Holiday");
            //modelBuilder.Entity<ComplainType>().ToTable("Post_ComplainType");
            //modelBuilder.Entity<ComplainStatus>().ToTable("Post_ComplainStatus");
            //modelBuilder.Entity<Complain>().ToTable("Post_Complain");
            //modelBuilder.Entity<ComplainHandle>().ToTable("Post_ComplainHandle");
            //modelBuilder.Entity<Incidents>().ToTable("Post_Incidents");
            //modelBuilder.Entity<FeeType>().ToTable("Post_FeeType");
            //modelBuilder.Entity<CompensationType>().ToTable("Post_CompensationType");
            //modelBuilder.Entity<Compensation>().ToTable("Post_Compensation");
            //modelBuilder.Entity<Bank>().ToTable("Core_Bank");
            //modelBuilder.Entity<Branch>().ToTable("Core_Branch");
            //modelBuilder.Entity<BankAccount>().ToTable("Core_BankAccount");
            //modelBuilder.Entity<AccountingAccount>().ToTable("Core_AccountingAccount");
            //modelBuilder.Entity<PackageStatus>().ToTable("Post_PackageStatus");
            //
            modelBuilder.Entity<Proc_GetListShipmentIncomingPayment>().ToTable(Proc_GetListShipmentIncomingPayment.ProcName);
            modelBuilder.Entity<Proc_UpdateCountPushLazada>().ToTable(Proc_UpdateCountPushLazada.ProcName);
            modelBuilder.Entity<Proc_ScanShipmentInPackage>().ToTable(Proc_ScanShipmentInPackage.ProcName);
            modelBuilder.Entity<Proc_ScanShipmentOutPackage>().ToTable(Proc_ScanShipmentOutPackage.ProcName);
            modelBuilder.Entity<Proc_GetInfoRemote>().ToTable(Proc_GetInfoRemote.ProcName);
            modelBuilder.Entity<Proc_ReportEmployeeOne>().ToTable(Proc_ReportEmployeeOne.ProcName);
            modelBuilder.Entity<Proc_CopyPriceService>().ToTable(Proc_CopyPriceService.ProcName);
            modelBuilder.Entity<Proc_CheckInfoInListGoods>().ToTable(Proc_CheckInfoInListGoods.ProcName);
            modelBuilder.Entity<Proc_BlockListGoods>().ToTable(Proc_BlockListGoods.ProcName);
            modelBuilder.Entity<Proc_SearchEntityByValue>().ToTable(Proc_SearchEntityByValue.ProcName);
            modelBuilder.Entity<Proc_CheckShipmentNumber>().ToTable(Proc_CheckShipmentNumber.ProcName);
            modelBuilder.Entity<Proc_GetShipmentByShipmentNumber>().ToTable(Proc_GetShipmentByShipmentNumber.ProcName);
            modelBuilder.Entity<Proc_PriceServiceDVGT>().ToTable(Proc_PriceServiceDVGT.ProcName);
            modelBuilder.Entity<Proc_UpdateCountPushVSE>().ToTable(Proc_UpdateCountPushVSE.ProcName);
            modelBuilder.Entity<Proc_TaskScheduler>().ToTable(Proc_TaskScheduler.ProcName);
            modelBuilder.Entity<Proc_UpdateAreaDistricts>().ToTable(Proc_UpdateAreaDistricts.ProcName);
            modelBuilder.Entity<Proc_GetAreaByPriceService>().ToTable(Proc_GetAreaByPriceService.ProcName);
            modelBuilder.Entity<Proc_PriceService>().ToTable(Proc_PriceService.ProcName);
            modelBuilder.Entity<Proc_PriceServicePlus>().ToTable(Proc_PriceServicePlus.ProcName);
            modelBuilder.Entity<Proc_Core_DeleteTable>().ToTable(Proc_Core_DeleteTable.ProcName);
            modelBuilder.Entity<Proc_LadingSchedule_Joined>().ToTable(Proc_LadingSchedule_Joined.ProcName);
            modelBuilder.Entity<Proc_RequestLadingSchedule_Joined>().ToTable(Proc_RequestLadingSchedule_Joined.ProcName);
            modelBuilder.Entity<Proc_LadingSchedule_report>().ToTable(Proc_LadingSchedule_report.ProcName);
            modelBuilder.Entity<Proc_ShipmentHubKeeping>().ToTable(Proc_ShipmentHubKeeping.ProcName);
            modelBuilder.Entity<Proc_ShipmentEmployeeKeeping>().ToTable(Proc_ShipmentEmployeeKeeping.ProcName);
            modelBuilder.Entity<Proc_ReportByCustomer>().ToTable(Proc_ReportByCustomer.ProcName);
            modelBuilder.Entity<Proc_ReportByPickupDelivery>().ToTable(Proc_ReportByPickupDelivery.ProcName);
            modelBuilder.Entity<Proc_ReportPayablesAndReceivablesByCustomer>().ToTable(Proc_ReportPayablesAndReceivablesByCustomer.ProcName);
            modelBuilder.Entity<Proc_DetectAddressTo>().ToTable(Proc_DetectAddressTo.ProcName);
            modelBuilder.Entity<Proc_ReportTransfer>().ToTable(Proc_ReportTransfer.ProcName);
            modelBuilder.Entity<Proc_ReportDeliveryByListGoods>().ToTable(Proc_ReportDeliveryByListGoods.ProcName);
            modelBuilder.Entity<Proc_ReportEmployee>().ToTable(Proc_ReportEmployee.ProcName);
            modelBuilder.Entity<Proc_ReportBroadcastEmployee>().ToTable(Proc_ReportBroadcastEmployee.ProcName);
            modelBuilder.Entity<Proc_GetListGoodsSendToHub>().ToTable(Proc_GetListGoodsSendToHub.ProcName);
            modelBuilder.Entity<Proc_ReportListGoods>().ToTable(Proc_ReportListGoods.ProcName);
            modelBuilder.Entity<Proc_GetReportBroadcastListGoodsByAppAndMobil>().ToTable(Proc_GetReportBroadcastListGoodsByAppAndMobil.ProcName);
            modelBuilder.Entity<Proc_GetByShipmentNumber>().ToTable(Proc_GetByShipmentNumber.ProcName);
            modelBuilder.Entity<Proc_CopyPriceList>().ToTable(Proc_CopyPriceList.ProcName);
            modelBuilder.Entity<Proc_ReportPrintShipment>().ToTable(Proc_ReportPrintShipment.ProcName);
            modelBuilder.Entity<Proc_GetHistoryPrintShipmentId>().ToTable(Proc_GetHistoryPrintShipmentId.ProcName);
            modelBuilder.Entity<Proc_ReportCancelShipment>().ToTable(Proc_ReportCancelShipment.ProcName);
            modelBuilder.Entity<Proc_DistrictFreeSelectPriceServiceDetail>().ToTable(Proc_DistrictFreeSelectPriceServiceDetail.ProcName);
            modelBuilder.Entity<Proc_DistrictSelectedPriceServiceDetail>().ToTable(Proc_DistrictSelectedPriceServiceDetail.ProcName);
            modelBuilder.Entity<Proc_ProvinceFreeSelectPriceServiceDetail>().ToTable(Proc_ProvinceFreeSelectPriceServiceDetail.ProcName);
            modelBuilder.Entity<Proc_ProvinceSelectedPriceServiceDetail>().ToTable(Proc_ProvinceSelectedPriceServiceDetail.ProcName);
            modelBuilder.Entity<Proc_FromProvincePriceServiceSelected>().ToTable(Proc_FromProvincePriceServiceSelected.ProcName);
            modelBuilder.Entity<Proc_GetReportShipmentsDeliveryByListGoodsIds>().ToTable(Proc_GetReportShipmentsDeliveryByListGoodsIds.ProcName);
            modelBuilder.Entity<Proc_GetExportDataReportSumary>().ToTable(Proc_GetExportDataReportSumary.ProcName);
            modelBuilder.Entity<Proc_GetShipmentToPrint>().ToTable(Proc_GetShipmentToPrint.ProcName);
            modelBuilder.Entity<Proc_GetBoxesByShipmentId>().ToTable(Proc_GetBoxesByShipmentId.ProcName);
            modelBuilder.Entity<Proc_ReportDebtPriceDetailByCustomer>().ToTable(Proc_ReportDebtPriceDetailByCustomer.ProcName);
            modelBuilder.Entity<Proc_ReportDebtCODDetailByCustomer>().ToTable(Proc_ReportDebtCODDetailByCustomer.ProcName);
            modelBuilder.Entity<Proc_ReportDebtPriceDetailByCustomerDetail>().ToTable(Proc_ReportDebtPriceDetailByCustomerDetail.ProcName);
            modelBuilder.Entity<Proc_ReportDebtCODDetailByCustomerDetail>().ToTable(Proc_ReportDebtCODDetailByCustomerDetail.ProcName);
            modelBuilder.Entity<Proc_ReportListGoodsPriceDetailByCustomer>().ToTable(Proc_ReportListGoodsPriceDetailByCustomer.ProcName);
            modelBuilder.Entity<Proc_UploadExcelWithTableValued>().ToTable(Proc_UploadExcelWithTableValued.ProcName);
            modelBuilder.Entity<Proc_GetListHubFromUser>().ToTable(Proc_GetListHubFromUser.ProcName);
            modelBuilder.Entity<Proc_GetListRequestShipmentByFilter>().ToTable(Proc_GetListRequestShipmentByFilter.ProcName);
            modelBuilder.Entity<Proc_PushImgPickupVSE>().ToTable(Proc_PushImgPickupVSE.ProcName);
            modelBuilder.Entity<Proc_GetShipmentNumberAuto>().ToTable(Proc_GetShipmentNumberAuto.ProcName);
            modelBuilder.Entity<Proc_UpdateShipmentNumberAuto>().ToTable(Proc_UpdateShipmentNumberAuto.ProcName);
            modelBuilder.Entity<Proc_GetBoxNumberAuto>().ToTable(Proc_GetBoxNumberAuto.ProcName);
            modelBuilder.Entity<Proc_GetDeadlineService>().ToTable(Proc_GetDeadlineService.ProcName);
            modelBuilder.Entity<Proc_AcceptCreateShipmentChild>().ToTable(Proc_AcceptCreateShipmentChild.ProcName);
            modelBuilder.Entity<Proc_AddDelay>().ToTable(Proc_AddDelay.ProcName);
            modelBuilder.Entity<Proc_GetLadingScheduleCurrent>().ToTable(Proc_GetLadingScheduleCurrent.ProcName);
            modelBuilder.Entity<Proc_GetEmailAddress>().ToTable(Proc_GetEmailAddress.ProcName);
            modelBuilder.Entity<Proc_LogSendEmail>().ToTable(Proc_LogSendEmail.ProcName);
            modelBuilder.Entity<Proc_GetListShipmentDelay>().ToTable(Proc_GetListShipmentDelay.ProcName);
            modelBuilder.Entity<Proc_GetNotificationMenu>().ToTable(Proc_GetNotificationMenu.ProcName);
            modelBuilder.Entity<Proc_ReportPickupDeltail>().ToTable(Proc_ReportPickupDeltail.ProcName);
            modelBuilder.Entity<Proc_ReportDeliveryDeltail>().ToTable(Proc_ReportDeliveryDeltail.ProcName);
            modelBuilder.Entity<Proc_ReportDeliveryFail>().ToTable(Proc_ReportDeliveryFail.ProcName);
            modelBuilder.Entity<Proc_ReportShipmentQuantity>().ToTable(Proc_ReportShipmentQuantity.ProcName);
            modelBuilder.Entity<Proc_ReportComplain>().ToTable(Proc_ReportComplain.ProcName);
            modelBuilder.Entity<Proc_ReportDiscountCustomer>().ToTable(Proc_ReportDiscountCustomer.ProcName);
            modelBuilder.Entity<Proc_ReportKPIBusiness>().ToTable(Proc_ReportKPIBusiness.ProcName);
            modelBuilder.Entity<Proc_ReportResultBusiness>().ToTable(Proc_ReportResultBusiness.ProcName);
            modelBuilder.Entity<Proc_ReportKPICustomer>().ToTable(Proc_ReportKPICustomer.ProcName);
            modelBuilder.Entity<Proc_ReportLadingSchedule>().ToTable(Proc_ReportLadingSchedule.ProcName);
            modelBuilder.Entity<Proc_ReportTruckTransfer>().ToTable(Proc_ReportTruckTransfer.ProcName);
            modelBuilder.Entity<Proc_ReportPaymentPickupUser>().ToTable(Proc_ReportPaymentPickupUser.ProcName);
            modelBuilder.Entity<Proc_ReportEmpReceiptIssue>().ToTable(Proc_ReportEmpReceiptIssue.ProcName);
            modelBuilder.Entity<Proc_ReportShipmentCOD>().ToTable(Proc_ReportShipmentCOD.ProcName);
            modelBuilder.Entity<Proc_GetTotalShipmentBox>().ToTable(Proc_GetTotalShipmentBox.ProcName);
            modelBuilder.Entity<Proc_ReportDeadline>().ToTable(Proc_ReportDeadline.ProcName);
            modelBuilder.Entity<Proc_ReportPrioritize>().ToTable(Proc_ReportPrioritize.ProcName);
            modelBuilder.Entity<Proc_ReportIncidents>().ToTable(Proc_ReportIncidents.ProcName);
            modelBuilder.Entity<Proc_ReportPickupDelivery>().ToTable(Proc_ReportPickupDelivery.ProcName);
            modelBuilder.Entity<Proc_DashboardPickup>().ToTable(Proc_DashboardPickup.ProcName);
            modelBuilder.Entity<Proc_DashboardTransfer>().ToTable(Proc_DashboardTransfer.ProcName);
            modelBuilder.Entity<Proc_DashboardDeliveryAndReturn>().ToTable(Proc_DashboardDeliveryAndReturn.ProcName);
            modelBuilder.Entity<Proc_DashboardService>().ToTable(Proc_DashboardService.ProcName);
            modelBuilder.Entity<Proc_ReportListGoodsShipment>().ToTable(Proc_ReportListGoodsShipment.ProcName);
            modelBuilder.Entity<Proc_GetListShipment>().ToTable(Proc_GetListShipment.ProcName);
            modelBuilder.Entity<Proc_GetListWarehousing>().ToTable(Proc_GetListWarehousing.ProcName);
            modelBuilder.Entity<Proc_GetShipmentCurrentEmp>().ToTable(Proc_GetShipmentCurrentEmp.ProcName);
            modelBuilder.Entity<Proc_GetRequestShipmentCurrentEmp>().ToTable(Proc_GetRequestShipmentCurrentEmp.ProcName);
            modelBuilder.Entity<Proc_GetCountShipmentByDealine>().ToTable(Proc_GetCountShipmentByDealine.ProcName);
            modelBuilder.Entity<Proc_RoleByComplainType>().ToTable(Proc_RoleByComplainType.ProcName);
            modelBuilder.Entity<Proc_GetRoleAndHubByUser>().ToTable(Proc_GetRoleAndHubByUser.ProcName);
            modelBuilder.Entity<Proc_CheckShipmentAcceptReturn>().ToTable(Proc_CheckShipmentAcceptReturn.ProcName);
            modelBuilder.Entity<Proc_ReportEmployeeCollected>().ToTable(Proc_ReportEmployeeCollected.ProcName);
            modelBuilder.Entity<Proc_ReportEmployeeCollecting>().ToTable(Proc_ReportEmployeeCollecting.ProcName);
            modelBuilder.Entity<Proc_GetBillUpdateInfo>().ToTable(Proc_GetBillUpdateInfo.ProcName);
            modelBuilder.Entity<Proc_GetBillDeliveryInfo>().ToTable(Proc_GetBillDeliveryInfo.ProcName);
            modelBuilder.Entity<Proc_UpdateCountImageVSE>().ToTable(Proc_UpdateCountImageVSE.ProcName);
            modelBuilder.Entity<Proc_GetBillPickupInfo>().ToTable(Proc_GetBillPickupInfo.ProcName);
            modelBuilder.Entity<Proc_GetListGoodsReceive>().ToTable(Proc_GetListGoodsReceive.ProcName);
            modelBuilder.Entity<Proc_GetDeliveryAndHubRouting>().ToTable(Proc_GetDeliveryAndHubRouting.ProcName);
            modelBuilder.Entity<Proc_GetInfoHubRouting>().ToTable(Proc_GetInfoHubRouting.ProcName);
            modelBuilder.Entity<Proc_CheckExistSealNumber>().ToTable(Proc_CheckExistSealNumber.ProcName);
            modelBuilder.Entity<Proc_GetShipmentByPackageId>().ToTable(Proc_GetShipmentByPackageId.ProcName);
            modelBuilder.Entity<Proc_GetShipmentPushRevenue>().ToTable(Proc_GetShipmentPushRevenue.ProcName);
            modelBuilder.Entity<Proc_GetListShipmentPayment>().ToTable(Proc_GetListShipmentPayment.ProcName);
            modelBuilder.Entity<Proc_GetShipmentListPaymentCustomer>().ToTable(Proc_GetShipmentListPaymentCustomer.ProcName);
            modelBuilder.Entity<Proc_AddShipmentToListPayment>().ToTable(Proc_AddShipmentToListPayment.ProcName);
            modelBuilder.Entity<Proc_ReportPercentDeliveryEmp>().ToTable(Proc_ReportPercentDeliveryEmp.ProcName);
            modelBuilder.Entity<Proc_GetListPackage>().ToTable(Proc_GetListPackage.ProcName);
            modelBuilder.Entity<Proc_GetListPackageBy>().ToTable(Proc_GetListPackageBy.ProcName);
            modelBuilder.Entity<Proc_GetPackageToPrint>().ToTable(Proc_GetPackageToPrint.ProcName);
            modelBuilder.Entity<Proc_UnInstallShipmentInListCustomerPayment>().ToTable(Proc_UnInstallShipmentInListCustomerPayment.ProcName);
            modelBuilder.Entity<Proc_GetShipmentStatusPush>().ToTable(Proc_GetShipmentStatusPush.ProcName); 
            modelBuilder.Entity<Proc_UpdateFastBooking>().ToTable(Proc_UpdateFastBooking.ProcName);
            modelBuilder.Entity<Proc_ReportByCus>().ToTable(Proc_ReportByCus.ProcName);
            modelBuilder.Entity<Proc_ReportByRevenueMonth>().ToTable(Proc_ReportByRevenueMonth.ProcName);
            modelBuilder.Entity<Proc_ReportByRevenueYear>().ToTable(Proc_ReportByRevenueYear.ProcName);
            modelBuilder.Entity<Proc_ReportHandleEmployee>().ToTable(Proc_ReportHandleEmployee.ProcName);
            modelBuilder.Entity<Proc_SaveLogReceiveData>().ToTable(Proc_SaveLogReceiveData.ProcName);
            modelBuilder.Entity<Proc_CheckRequestShipmentCompleted>().ToTable(Proc_CheckRequestShipmentCompleted.ProcName);
            modelBuilder.Entity<Proc_ReportListShipment>().ToTable(Proc_ReportListShipment.ProcName);
            modelBuilder.Entity<Proc_ReportListGoodsDetail>().ToTable(Proc_ReportListGoodsDetail.ProcName);
            modelBuilder.Entity<Proc_CheckExistImageDelivery>().ToTable(Proc_CheckExistImageDelivery.ProcName);
            modelBuilder.Entity<Proc_GetListShipmentUpdateStatusTPL>().ToTable(Proc_GetListShipmentUpdateStatusTPL.ProcName);
            modelBuilder.Entity<Proc_ReportUpdateReceiveInformation>().ToTable(Proc_ReportUpdateReceiveInformation.ProcName);
            modelBuilder.Entity<Proc_ReportPaymentEmployees>().ToTable(Proc_ReportPaymentEmployees.ProcName);
            modelBuilder.Entity<Proc_ReportShipmentVersion>().ToTable(Proc_ReportShipmentVersion.ProcName);
            modelBuilder.Entity<Proc_ReportExpenseReceiveMoney>().ToTable(Proc_ReportExpenseReceiveMoney.ProcName);
            modelBuilder.Entity<Proc_ReportRevenueCustomner>().ToTable(Proc_ReportRevenueCustomner.ProcName);
            modelBuilder.Entity<Proc_ReportLandingScheduleReport>().ToTable(Proc_ReportLandingScheduleReport.ProcName);
            modelBuilder.Entity<Proc_GetDiscount>().ToTable(Proc_GetDiscount.ProcName);
            modelBuilder.Entity<Proc_GetAllDiscount>().ToTable(Proc_GetAllDiscount.ProcName);
            modelBuilder.Entity<Proc_ReportAllSchedule2>().ToTable(Proc_ReportAllSchedule2.ProcName);
            modelBuilder.Entity<Proc_ReportRevenueCustomer>().ToTable(Proc_ReportRevenueCustomer.ProcName);
            modelBuilder.Entity<Proc_GetPaymentTargetCODConversion>().ToTable(Proc_GetPaymentTargetCODConversion.ProcName); 
            //
            //modelBuilder.Entity<User>()
            //            .HasOne(user => user.Hub)
            //            .WithMany(user => user.Users);

            //modelBuilder.Entity<Hub>()
            //            .HasOne(hub => hub.CenterHub)
            //            .WithMany(hub => hub.PoHubs);
        }

        #region Table
        #endregion
    }
}
