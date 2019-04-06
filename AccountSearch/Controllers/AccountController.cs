using System.Collections.Generic;
using System.Text.RegularExpressions;
using AccountSearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountSearch.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {        
        [HttpGet]        
        public ActionResult<string> Get()
        {
            return string.Empty;
        }

        [HttpGet("accounts/{*searchTerm}")]
        public ActionResult<List<Account>> GetAccounts(string searchTerm)
        {
            var dataProvider = new DataProvider.DataProvider();

            if (!TryValidateSearchTerm(searchTerm, out var validationMessage))
            {
                return UnprocessableEntity(validationMessage);
            }

            return dataProvider.GetAccounts(searchTerm);
        }

        [HttpGet("accountbalances/{*searchTerm}")]
        public ActionResult<List<AccountBalance>> GetBalances(string searchTerm)
        {
            var dataProvider = new DataProvider.DataProvider();

            if (!TryValidateSearchTerm(searchTerm, out var validationMessage))
            {
                return UnprocessableEntity(validationMessage);
            }

            return dataProvider.GetAccountBalances(searchTerm);
        }

        // If we had more than the two gets, or our validation becomes very complex we should
        // think about moving this to a validation class.
        private bool TryValidateSearchTerm(string searchTerm, out string validationProblemMessage)
        {
            validationProblemMessage = string.Empty;

            if (string.IsNullOrEmpty(searchTerm)) return true;

            if (searchTerm.Length > 20)
            {
                validationProblemMessage = "Too many characters passed in search term. 20 character maximum";
                return false;
            }

            var regex = new Regex("[^a-zA-Z0-9 ]");

            if (regex.Matches(searchTerm).Count > 0)
            {
                validationProblemMessage = "Your search term can only contain letters and numbers.";
                return false;
            }

            return true;
        }
    }
}
