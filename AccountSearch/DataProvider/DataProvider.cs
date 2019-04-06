using System.Collections.Generic;
using System.IO;
using System.Linq;
using AccountSearch.Models;
using CodeTitans.JSon;

namespace AccountSearch.DataProvider
{
    public class DataProvider : IDataProvider
    {
        private IJSonObject _jsonAccountData;
        private IJSonObject _jsonBalanceData;

        public List<Account> GetAccounts(string searchData = "")
        {
            // We load the json data everytime... REST services shouldn't hold server side state between calls
            // In a real application this data would come from a DB and the connection pooling would be handled
            // by the server, but for our stateless server using json files we need to load for each call.
            LoadJsonData();

            var accountData = new List<Account>();
            var searchableJsonData = _jsonAccountData.ArrayItems;

            if (!string.IsNullOrEmpty(searchData))
            {
                searchableJsonData = _jsonAccountData.ArrayItems.Where(i => i["Number"].ToString() == searchData || i["Name"].ToString() == searchData);
            }

            foreach (var accountItem in searchableJsonData)
            {
                accountData.Add(
                    new Account(accountItem["Id"].Int32Value,
                                accountItem["Number"].ToString(),
                                accountItem["Type"].ToString(),
                                accountItem["Name"].ToString(),
                                accountItem["Status"].ToString()));
            }

            return accountData;
        }

        public List<AccountBalance> GetAccountBalances(string searchData = "")
        {
            var accounts = GetAccounts(searchData);
            var accountBalanceData = new List<AccountBalance>();
            var searchableJson = _jsonBalanceData["Balances"].ArrayItems;

            foreach (var account in accounts)
            {
                account.MaskAccountNumber = true;

                // Looking at the data there is the possibilty that an account has no active balance...
                // Given the specification doesn't tell us what we need to do in this situation we will return the account and default all the other values...
                var accountBalance = searchableJson.FirstOrDefault(i => i["Id"].Int32Value == account.Id);
                var balance = accountBalance == null ? null : accountBalance["Balance"].ObjectValue == null ? null : (decimal?)accountBalance["Balance"].DecimalValue;
                var overdraft = accountBalance == null ? null : accountBalance["Overdraft"].ObjectValue == null ? null : (decimal?)accountBalance["Overdraft"].DecimalValue;

                accountBalanceData.Add(new AccountBalance(account, balance, overdraft));
            }

            return accountBalanceData;
        }

        private void LoadJsonData()
        {
            var jsonReader = new JSonReader();
            _jsonAccountData = jsonReader.ReadAsJSonObject(File.ReadAllText(@"Resources\accounts.json"));
            _jsonBalanceData = jsonReader.ReadAsJSonObject(File.ReadAllText(@"Resources\accountbalances.json"));
        }
    }
}
