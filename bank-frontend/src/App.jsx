import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import React from 'react'
import "bootstrap/dist/css/bootstrap.min.css";

function App() {
  const [accounts, setAccounts] = React.useState([]);
  const [owner, setOwner] = React.useState("");
  const [balance, setBalance] = React.useState(0);

  const fetchAccounts = async () => {
    try {
      const response = await fetch('http://localhost:3000/accounts');
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      setAccounts(data);
    } catch (error) {
      console.error('Error fetching accounts:', error);
    }
  }
  
  const createAccount = async () => {
    if (!owner) {
      alert("Please enter an owner name.");
      return;
    }
    if (balance <= 0) {
      alert("Balance cannot be negative.");
      return;
    }

    if (isNaN(balance) || balance === "") {
      alert("Please enter a valid balance.");
      return;
    }

    if (parseFloat(balance) <= 0) {
      alert("Balance cannot be negative.");
      return;
    }
    if (accounts.some(account => account.owner === owner)) {
      alert("An account with this owner already exists.");
      return;
    }
    if (owner.length < 3) {
      alert("Owner name must be less than 50 characters long.");
      return;
    }
    if (owner.length > 50) {
      alert("Owner name must be less than 50 characters long.");
      return;
    }

    await fetch('http://localhost:3000/accounts', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ owner, balance }),
    });

    setOwner("");
    setBalance(0);
    fetchAccounts();
}

// function to handle deposit
const handleDeposit = async (accountId, amount) => {
  const amount = prompt("Enter the amount to deposit:");
  if (amount <= 0 || isNaN(amount)) {
    alert("Please enter a valid deposit amount.");
    return;
  }

  await fetch(`http://localhost:3000/accounts/${accountId}/deposit`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ amount }),
  });

  fetchAccounts();
}

// function to handle withdraw
const handleWithdraw = async (accountId, amount) => {
  const amount = prompt("Enter the amount to withdraw:");
  if (amount <= 0 || isNaN(amount)) {
    alert("Please enter a valid withdrawal amount.");
    return;
  }

  await fetch(`http://localhost:3000/accounts/${accountId}/withdraw`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ amount }),
  });

  fetchAccounts();
}

//function to handle account update 
const handleUpdate = async (accountId) => {
  const newOwner = prompt("Enter the new owner name:");
  if (!newOwner || newOwner.length < 3 || newOwner.length > 50) {
    alert("Owner name must be between 3 and 50 characters long.");
    return;
  }

  await fetch(`http://localhost:3000/accounts/${accountId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ owner: newOwner }),
  });

  fetchAccounts();
}

//functions to handle delete 
const handleDelete = async (accountId) => {
  if (window.confirm("Are you sure you want to delete this account?")) {
    await fetch(`http://localhost:3000/accounts/${accountId}`, {
      method: 'DELETE',
    });
    fetchAccounts();
  }

}

  useEffect(() => {
    fetchAccounts();
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <img src={viteLogo} className="logo" alt="Vite logo" />
        <img src={reactLogo} className="logo react" alt="React logo" />
        <h1>Banking App</h1>
      </header>
      <main>
        <div className="container">
          <h2>Create Account</h2>
          <form onSubmit={(e) => { e.preventDefault(); createAccount(); }}>
            <input
              type="text"
              placeholder="Owner Name"
              value={owner}
              onChange={(e) => setOwner(e.target.value)}
              required
            />
            <input
              type="number"
              placeholder="Initial Balance"
              value={balance}
              onChange={(e) => setBalance(e.target.value)}
              required
            />
            <button type="submit">Create Account</button>
          </form>

          <h2>Accounts</h2>
          <table className="table">
            <thead>
              <tr>
                <th>Owner</th>
                <th>Balance</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {accounts.map(account => (
                <tr key={account.id}>
                  <td>{account.owner}</td>
                  <td>{account.balance}</td>
                  <td>

                      <button className ="btn btn-success btn-sn" type='submit' onClick={()=> handleDeposit(account.id)}>Deposit</button>
                      <button className ="btn btn-danger btn-sn" type='submit' onClick={()=> handleWithdraw(account.id)}>Withdraw</button>
                      <button className ="btn btn-primary btn-sn" type='submit' onClick={()=> handleUpdate(account.id)}>Update Owner</button>
                      <button className ="btn btn-danger btn-sn" type='submit' onClick={()=> handleDelete(account.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

        </div>
      </main>
    </div>
  )
 }