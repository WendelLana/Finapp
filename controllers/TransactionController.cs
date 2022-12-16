using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ControleFinanceiro.models;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.controllers
{
    public class TransactionController
    {
        protected readonly Context _context;
        private readonly MainWindow _mainWindow;
        public TransactionController(Context context)
        {
            _context = context;
            try
            {
                _mainWindow = (MainWindow)Application.Current.MainWindow;
            } catch (Exception e)
            {

            }
        }

        public List<Transaction> GetAll()
        {
            return _context.Transactions.Where(t => !t.recorrente).Include("Category").ToList();
        }

        public List<Transaction> GetAllRecurrent()
        {
            return _context.Transactions.Where(t => t.recorrente).Include("Category").ToList();
        }

        public List<Transaction> GetAllTransactionsOnly()
        {
            return _context.Transactions.ToList();
        }

        public Transaction GetById(Guid id)
        {
            return _context.Transactions.FirstOrDefault(obj => obj.id == id);
        }

        public bool Add(Transaction transaction)
        {
            if(transaction.recorrente)
            {
                transaction.date = new DateTime(transaction.date.Year, transaction.date.Month,
                    DateTime.DaysInMonth(transaction.date.Year, transaction.date.Month), 23, 59, 59);
            }
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            if (_mainWindow != null) _mainWindow.updateBalanceText();
            return true;
        }

        public bool Remove(Transaction transation)
        {
            _context.Transactions.Remove(transation);
            _context.SaveChanges();
            if (_mainWindow != null) _mainWindow.updateBalanceText();
            return true;
        }

        public bool Update(Transaction transation)
        {
            _context.Transactions.Update(transation);
            _context.SaveChanges();
            if (_mainWindow != null) _mainWindow.updateBalanceText();
            return true;
        }

        public decimal GetBalance()
        {
            decimal balance = 0;
            var items = _context.Transactions.ToList();

            items.ForEach(obj =>
            {
                if(obj.recorrente)
                {
                    var date1 = DateTime.Now; var date2 = obj.date;
                    var value = (((date1.Year - date2.Year) * 12) + date1.Month - date2.Month + 1) * obj.value;
                    balance += obj.transactionType == "I" ?
                        Convert.ToDecimal(value) : -Convert.ToDecimal(value);
                }
                else
                {
                    balance += obj.transactionType == "I" ?
                        Convert.ToDecimal(obj.value) : -Convert.ToDecimal(obj.value);
                }
            });

            return balance;
        }

        public decimal GetBalanceMonth()
        {
            decimal balance = 0;
            var items = _context.Transactions.Where(t => t.date.Month == DateTime.Now.Month || t.recorrente)
                .ToList();

            items.ForEach(obj =>
            {
                balance += obj.transactionType == "I" ?
                    Convert.ToDecimal(obj.value) : -Convert.ToDecimal(obj.value);
            });

            return balance;
        }

        public List<Transaction> GetByMonthAndYear(int month, int year)
        {
            return _context.Transactions.Where(obj => obj.date.Month == month && obj.date.Year == year).Include("Category").ToList();
        }

        public List<Transaction> GetAllByYear(int year)
        {
            return _context.Transactions.Where(obj => obj.date.Year == year).Include("Category").ToList();
        }
    }
}
