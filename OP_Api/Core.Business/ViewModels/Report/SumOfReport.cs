using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class SumOfReport
    {
        public double? SumOfTotalPriceByReportListGoodsPriceCustomer { get; set; } // tổng cước trong báo cáo bảng kê cước chi tiết theo khách hàng
        //
        public double? SumOfTotalBeforeByReportDebtPriceDetailCustomer { get; set; } // tổng dư nợ đầu kỳ trong báo cáo công nợ cước chi tiết theo khách hàng
        public double? SumOfTotalPriceByReportDebtPriceDetailCustomer { get; set; } // tổng cước phát sinh trong kỳ trong báo cáo công nợ cước chi tiết theo khách hàng
        public double? SumOfTotalPricePaidByReportDebtPriceDetailCustomer { get; set; } // tổng cước thanh toán trong kỳ trong báo cáo công nợ cước chi tiết theo khách hàng
        public double? SumOfTotalAfterPaidByReportDebtPriceDetailCustomer { get; set; } // tổng dư có cuối kỳ trong báo cáo công nợ cước chi tiết theo khách hàng
        //
        public double? SumOfTotalBeforeByReportDebtPriceDetailCustomerDetail { get; set; } // tổng dư nợ đầu kỳ trong báo cáo công nợ cước chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalCreditBalanceBeforeByReportDebtPriceDetailCustomerDetail { get; set; } // tổng dư có đầu kỳ trong báo cáo công nợ cước chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalPriceByReportDebtPriceDetailCustomerDetail { get; set; } // tổng cước phát sinh trong kỳ trong báo cáo công nợ cước chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalPricePaidByReportDebtPriceDetailCustomerDetail { get; set; } // tổng thanh toán trong báo cáo công nợ cước chi tiết theo khách hàng - xem chi tiết bill
        //
        public double? SumOfTotalCODByReportDebtCODDetailCustomer { get; set; } // tổng COD trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalCODReturnByReportDebtCODDetailCustomer { get; set; } // tổng COD hoàn trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalCODChargedByReportDebtCODDetailCustomer { get; set; } // tổng đã thu COD trong kỳ trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalCODNotChargedByReportDebtCODDetailCustomer { get; set; } // tổng đã chưa thu COD trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalCODPaidByReportDebtCODDetailCustomer { get; set; } // tổng đã thanh toán COD trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalCODNotPaidByReportDebtCODDetailCustomer { get; set; } // tổng chưa thanh toán COD trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalPriceByReportDebtCODDetailCustomer { get; set; } // tổng cước trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalPricePaidByReportDebtCODDetailCustomer { get; set; } // tổng đã thanh toán cước trong báo cáo công nợ COD chi tiết theo khách hàng
        public double? SumOfTotalPriceNotPaidByReportDebtCODDetailCustomer { get; set; } // tổng chưa thanh toán cước trong báo cáo công nợ COD chi tiết theo khách hàng
        //
        public double? SumOfTotalCODByReportDebtCODDetailCustomerDetail { get; set; } // tổng COD trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalCODReturnByReportDebtCODDetailCustomerDetail { get; set; } // tổng COD hoàn trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalCODChargedByReportDebtCODDetailCustomerDetail { get; set; } // tổng đã thu COD trong kỳ trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalCODNotChargedByReportDebtCODDetailCustomerDetail { get; set; } // tổng đã chưa thu COD trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalCODPaidByReportDebtCODDetailCustomerDetail { get; set; } // tổng đã thanh toán COD trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalCODNotPaidByReportDebtCODDetailCustomerDetail { get; set; } // tổng chưa thanh toán COD trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalPriceByReportDebtCODDetailCustomerDetail { get; set; } // tổng cước trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalPricePaidByReportDebtCODDetailCustomerDetail { get; set; } // tổng đã thanh toán cước trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
        public double? SumOfTotalPriceNotPaidByReportDebtCODDetailCustomerDetail { get; set; } // tổng chưa thanh toán cước trong báo cáo công nợ COD chi tiết theo khách hàng - xem chi tiết bill
    }
}