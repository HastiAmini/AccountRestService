using AccountSearch.Models;
using System.Collections.Generic;

namespace AccountSearch.DataProvider
{
    interface IDataProvider
    {
        List<Account> GetAccounts(string searchData = "");
        List<AccountBalance> GetAccountBalances(string searchData = "");
    }
}
