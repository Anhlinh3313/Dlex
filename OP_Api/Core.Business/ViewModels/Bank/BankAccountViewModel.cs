using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class BankAccountViewModel : SimpleViewModel<BankAccountViewModel, BankAccount>
    {
        public BankAccountViewModel() {}
        public int BranchId { get; set; }
        public int? AccountingAccountId { get; set; }
    }
}
