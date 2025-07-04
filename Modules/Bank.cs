using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
public class Bank
{
// Core logic layer for managing the lifecycle of bank accounts
// Handles file persistence, business rules, and state changes

// Path to the file where accounts are saved as JSON
private const string FilePath = "accounts.json";

// List of all bank accounts in memory
private List<BankAccount> _accounts;

// On construction, loads the account data from disk
public Bank()
{
// Load accounts from the file
_accounts = LoadAccounts();
}

// Returns a list of all active (non-deleted) accounts
public List<BankAccount> GetAccounts()
{
// Filter out deleted accounts
return _accounts.Where(a => !a.IsDeleted).ToList();
}

// Retrieves a single account by ID if it is not deleted
public BankAccount? GetAccount(int id)
{
// Find the account by ID
var account = _accounts.FirstOrDefault(a => a.Id == id);
// Return null if the account is deleted
return account?.IsDeleted == true ? null : account;
}

// Creates a new account with an owner and an optional initial balance
public BankAccount CreateAccount(string owner, decimal initialBalance = 0)
{
// Generate a new ID for the account
var newId = _accounts.Count > 0 ? _accounts.Max(a => a.Id) + 1 : 1;
// Create a new account object
var newAccount = new BankAccount(newId, owner, initialBalance);
// Add the new account to the list
_accounts.Add(newAccount);
// Save the accounts to disk
SaveAccounts();
// Return the newly created account
return newAccount;
}

// Deposits funds into a specific account
public void Deposit(int id, decimal amount)
{
// Find the account by ID
var account = GetAccount(id);
// If the account is found and not deleted, deposit the funds
if (account != null)
{
account.Deposit(amount);
// Save the accounts to disk
SaveAccounts();
}
}

// Withdraws funds from a specific account if balance allows
public void Withdraw(int id, decimal amount)
{
// Find the account by ID
var account = GetAccount(id);
// If the account is found and not deleted, withdraw the funds
if (account != null)
{
account.Withdraw(amount);
// Save the accounts to disk
SaveAccounts();
}
}

// Marks the account as deleted (soft deletion, data remains on disk)
public void DeleteAccount(int id)
{
// Find the account by ID
var account = GetAccount(id);
// If the account is found and not deleted, mark it as deleted
if (account != null)
{
account.IsDeleted = true;
// Save the accounts to disk
SaveAccounts();
}
}

// Updates the owner's name for a given account
public void UpdateAccountOwner(int id, string newOwner)
{
// Find the account by ID
var account = GetAccount(id);
// If the account is found and not deleted, update the owner
if (account != null)
{
account.Owner = newOwner;
// Save the accounts to disk
SaveAccounts();
}
}

// Returns all active accounts matching the specified owner name
public List<BankAccount> GetAccountsByOwner(string owner)
{
// Filter accounts by owner name and exclude deleted ones
return _accounts.Where(a => a.Owner != null && a.Owner.Equals(owner, StringComparison.OrdinalIgnoreCase) && !a.IsDeleted).ToList();
}

// Saves all account data to disk as formatted JSON
private void SaveAccounts()
{
// Serialize the accounts to JSON
var json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });
// Write the JSON to the file
File.WriteAllText(FilePath, json);
}

// Loads accounts from the JSON file
// Also updates the internal account ID counter to continue issuing unique IDs
private List<BankAccount> LoadAccounts()
{
// Check if the file exists
if (File.Exists(FilePath))
{
// Read the JSON from the file
var json = File.ReadAllText(FilePath);
// Deserialize the JSON into a list of accounts
var accounts = JsonSerializer.Deserialize<List<BankAccount>>(json);
// Return the list of accounts
return accounts ?? new List<BankAccount>();
}
// If the file does not exist, return an empty list
return new List<BankAccount>();
}}

