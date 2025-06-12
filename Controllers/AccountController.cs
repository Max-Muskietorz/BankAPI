using Microsoft.AspNetCore.Mvc;
using BankAPI.Models;

namespace BankAPI.Controllers
{
    [ApiController]

    [Route("api/[controller]")]

    public class AccountsController : ControllerBase
    {
        private readonly Bank _bank;
        public AccountsController(Bank bank)
        {
            _bank = bank;
        }
        
        // create new account with owner name and optional initial balance
        [HttpPost]
        [Route("create")]
        public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
        {
            if (string.IsNullOrEmpty(request.Owner))
            {
                return BadRequest("Owner name cannot be empty");
            }

            var account = new BankAccount(request.Owner, request.InitialBalance);
            // Save the account to the database or in-memory storage
            // ...

            return Ok(account);
        }

        // get all accounts
        [HttpGet]
        [Route("all")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _bank.GetAccounts();
            return Ok(accounts);
        }

        // get account by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAccountById(int id)
        {
            try
            {
                var account = _bank.GetAccountById(id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //get account by owner name
        [HttpGet]
        [Route("owner/{owner}")]
        public IActionResult GetAccountByOwner(string owner)
        {
            var accounts = _bank.GetAccounts().Where(a => a.Owner.Equals(owner, StringComparison.OrdinalIgnoreCase)).ToList();
            if (accounts.Count == 0)
            {
                return NotFound("No accounts found for the specified owner");
            }
            return Ok(accounts);
        }

        //update account owner
        [HttpPut]
        [Route("{id}/update")]
        public IActionResult UpdateAccountOwner(int id, [FromBody] string newOwner)
        {
            try
            {
                _bank.UpdateOwner(id, newOwner);
                return Ok("Account owner updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // deposit money into account
        [HttpPost]
        [Route("{id}/deposit")]
        public IActionResult Deposit(int id, [FromBody] decimal amount)
        {
            try
            {
                _bank.Deposit(id, amount);
                return Ok("Deposit successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // withdraw money from account
        [HttpPost]
        [Route("{id}/withdraw")]
        public IActionResult Withdraw(int id, [FromBody] decimal amount)
        {
            try
            {
                _bank.Withdraw(id, amount);
                return Ok("Withdrawal successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete account
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            try
            {
                _bank.DeleteAccount(id);
                return Ok("Account deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}

