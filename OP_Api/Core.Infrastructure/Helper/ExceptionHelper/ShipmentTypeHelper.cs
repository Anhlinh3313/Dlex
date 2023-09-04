using System;
namespace Core.Infrastructure.Helper.ExceptionHelper
{
    public static class ShipmentTypeHelper
    {
        public const string WaitingToPickup = "waitingforpickup";
        public const string AssignPickup = "assignpickup";
        public const string Picking = "picking";
        public const string PickupComplete = "pickupcomplete";
        public const string PickupCancel = "pickupcancel";
        public const string Pickup = "pickup";
        public const string UpdatePickup = "updatepickup";
        public const string WaitingToDelivery = "waitingfordelivery";
        public const string WaitingToDeliveryAndHubOther = "waitingfordeliveryandhubother";
        public const string AssignDelivery = "assigndelivery";
        public const string Delivering = "delivering";
        public const string DeliveryComplete = "deliverycomplete";
        public const string DeliveryFail = "deliveryfail";
        public const string DeliveryCancel = "deliverycancel";
        public const string Delivery = "delivery";
        public const string UpdateDelivery = "updatedelivery";
        public const string Transfer = "transfer";
        public const string TransferAllHub = "transferallhub";
        public const string Transferring = "transferring";
        public const string UpdateTransfer = "updatetransfer";
        public const string ReShipTransfer = "reShipTransfer";
        public const string Return = "return";
        public const string UpdateReturn = "updatereturn";
        public const string WaitingToPack = "waitingtopack";
        public const string PackPackage = "packpackage";
        public const string OpenPackage = "openpackage";
        public const string HubConfirmMoneyFromRider = "hubconfirmmoneyfromrider";
        public const string ParentHubConfirmMoneyFromHub = "parenthubconfirmmoneyfromhub";
        public const string AccountantConfirmMoneyFromHub = "accountantconfirmmoneyfromhub";
        public const string TreasurerConfirmMoneyFromAccountant = "treasurerconfirmmoneyfromaccountant";
        public const string TransferTPL = "transfertpl";
        public const string Inventory = "inventory";
        public const string ReportSumnary = "reportsummary";
        public const string IsReturn = "isreturn";
        public const string CancelReturn = "cancelreturn";
        public const string CODReadyRecive= "codreadyrecive";
        public const string CODRecivedReadyPayment = "codrecivereadypayment";
        public const string PriceReadyPayment = "pricereadypayment";
        public const string ProcessError = "processerror";
        public const string WaitingHandling = "waitinghandling";
        public const string Incident = "incident";
        public const string Compensation = "compensation";

        public class CreateShipmentType
        {
            public const int Normal = 1;
            public const int Related = 2;
            public const int Return = 3;
            public const int RetrunDOC = 4;
        }
    }
}
