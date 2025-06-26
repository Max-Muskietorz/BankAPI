using Microsoft.AspNetCore.Mvc;

[ApiController]

[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
// Reference to the Bank logic layer for managing account data
//- Create new account with owner name and optional initial balance
private readonly Bank _bank;
public AccountsController(Bank bank)
{
// Inject the Bank service into the controller
_bank = bank;
}
//- Get all accounts

[HttpGet]
public ActionResult<List<BankAccount>> GetAccounts()
{
// Return the list of all accounts
return Ok(_bank.GetAccounts());
}

//- Get account by ID
[HttpGet("{id}")]
public ActionResult<BankAccount> GetAccount(int id)
{
// Retrieve the account by ID
var account = _bank.GetAccount(id);
// If not found, return NotFound
if (account == null)
{
return NotFound();
}
// Return the found account
return Ok(account);
}
//- Create new account
[HttpPost("create")]
public ActionResult<BankAccount> CreateAccount([FromBody] BankAccount newAccount)
{
// Validate the input
if (newAccount == null || string.IsNullOrWhiteSpace(newAccount.Owner))
{
return BadRequest("Invalid account data.");
}
// Create the new account using the Bank service
var createdAccount = _bank.CreateAccount(newAccount.Owner, newAccount.Balance);
// Return the created account with a Created response
return CreatedAtAction(nameof(GetAccount), new { id = createdAccount.Id }, createdAccount);
}//- Deposit funds into account
[HttpPost("{id}/deposit")]
public ActionResult Deposit(int id, [FromBody] decimal amount)
{
// Validate the deposit amount
if (amount <= 0)
{
return BadRequest("Deposit amount must be positive.");
}
// Perform the deposit using the Bank service
_bank.Deposit(id, amount);
// Return NoContent to indicate success
return NoContent();
}
//- Withdraw funds from account
[HttpPost("{id}/withdraw")]
public ActionResult Withdraw(int id, [FromBody] decimal amount)
{
// Validate the withdrawal amount
if (amount <= 0)
{
return BadRequest("Withdrawal amount must be positive.");
}
// Perform the withdrawal using the Bank service
_bank.Withdraw(id, amount);
// Return NoContent to indicate success
return NoContent();
}
//- Delete account (soft delete)

[HttpDelete("{id}/delete")]
public ActionResult DeleteAccount(int id)
{
// Attempt to delete the account using the Bank service
_bank.DeleteAccount(id);
// Return NoContent to indicate success
return NoContent();
}
}
