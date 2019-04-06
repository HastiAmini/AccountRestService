using System.Runtime.Serialization;

namespace AccountSearch.Models
{
    [DataContract]
    public class AccountBalance
    {
        [DataMember] public Account Account { get; private set; }
        [DataMember] public decimal? Balance { get; private set; }
        [DataMember] public decimal? Overdraft { get; private set; }

        public AccountBalance(Account account, decimal? balance, decimal? overdraft)
        {
            Account = account;
            Balance = balance;
            Overdraft = overdraft;
        }
    }
}
