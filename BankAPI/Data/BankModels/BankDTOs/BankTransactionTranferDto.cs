using System;
using System.Collections.Generic;

namespace BankAPI.Data.BankModels
{
    public partial class BankTransactionTransferDto
    {
        public int AccountId { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int ExternalAccount { get; set; }
    }
}
