public class BankAccount
{
public int Id { get; set; }
public string? Owner { get; set; }
public decimal Balance { get; set; }
public bool IsDeleted { get; set; }

// Add the required constructor
public BankAccount(int id, string owner, decimal initialBalance)
{
Id = id;
Owner = owner;
Balance = initialBalance;
IsDeleted = false;
}

// Empty constructor needed for JSON deserialisation
public BankAccount()
{
// This constructor is intentionally left empty
// It is used for JSON deserialization purposes
}

// Deposits money into the account (no effect if account is deleted)
public void Deposit(decimal amount)
{
// Check if the account is not deleted
if (!IsDeleted)
{
// Add the deposit amount to the balance
Balance += amount;
}
}

// Withdraws money from the account if sufficient funds are available
public void Withdraw(decimal amount)
{
// Check if the account is not deleted and has sufficient funds
if (!IsDeleted && Balance >= amount)
{
// Subtract the withdrawal amount from the balance
Balance -= amount;
}
}


// Marks this account as deleted (soft deletion)
public void MarkAsDeleted()
{
// Set the IsDeleted property to true
IsDeleted = true;
}

// Updates the owner name for this account (only if active and valid name provided)
public void UpdateOwner(string newOwner)
{
// Check if the account is not deleted and the new owner name is valid
if (!IsDeleted && !string.IsNullOrWhiteSpace(newOwner))
{
// Update the owner name
Owner = newOwner;
}
}

// Returns true if the account is active (not deleted)
public bool IsActive()
{
// Check if the account is not deleted
return !IsDeleted;
}


// Returns a formatted string with account information (for display/logging)
public override string ToString()
{
// Format the account information as a string
return $"Account ID: {Id}, Owner: {Owner}, Balance: {Balance:C}, Deleted: {IsDeleted}";
}
}