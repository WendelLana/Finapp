using ControleFinanceiro.models;
using ControleFinanceiro.controllers;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FinappXunit;

public class UnitTests
{
    private string dbPath = Directory.GetCurrentDirectory() + "/TestDatabase.db";

    [Fact]
    public void TestControllers()
    {
        if(File.Exists(dbPath))
        {
            File.Delete(dbPath);
        }

        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        optionsBuilder.UseSqlite("Data source = TestDatabase.db");
        Context _context = new(optionsBuilder.Options);

        CategoryController categoryController = new(_context);
        List<Category> expectedListCat = new();

        Guid cId = Guid.NewGuid();
        Category category = new()
        {
            id = cId,
            name = "Categoria 1",
            pack = "PackIconMaterial",
            icon = "Car",
            color = "#FFFFFF",
            transactionType = "O"
        };
        expectedListCat.Add(category);

        Guid cId2 = Guid.NewGuid();
        category = new()
        {
            id = cId2,
            name = "Categoria 2",
            pack = "PackIconMaterial",
            icon = "CreditCardOutline",
            color = "#FFFFFF",
            transactionType = "I"
        };
        expectedListCat.Add(category);

        for (int i = 0; i < expectedListCat.Count; i++)
        {
            categoryController.Add(expectedListCat[i]);
        }

        List<Category> retrievedListCat = categoryController.GetAll();
        Assert.Equal(retrievedListCat, expectedListCat);

        TransactionController transactionController = new(_context);
        List<Transaction> expectedList = new();

        Transaction t = new();
        Guid id = Guid.NewGuid(); 
        t.id = id; t.date = new DateTime(2022, 12, 11, 11, 26, 50); t.value = 3000; t.description = "Laptop"; t.categoryId = cId; t.transactionType = "O"; t.recorrente = false;
        expectedList.Add(t);

        t = new Transaction();
        id = Guid.NewGuid();
        t.id = id; t.date = new DateTime(2022, 6, 30, 23, 59, 59); t.value = 1500; t.description = "Aluguel"; t.categoryId = cId; t.transactionType = "O"; t.recorrente = true;
        expectedList.Add(t);

        t = new Transaction();
        id = Guid.NewGuid();
        t.id = id; t.date = new DateTime(2022, 9, 30, 23, 59, 59); t.value = 5000; t.description = "Salário"; t.categoryId = cId2; t.transactionType = "I"; t.recorrente = true;
        expectedList.Add(t);

        for (int i = 0; i < expectedList.Count; i++)
        {
            transactionController.Add(expectedList[i]);
        }

        List<Transaction> retrievedList = transactionController.GetAllTransactionsOnly();        
        Assert.Equal(retrievedList, expectedList);

        t.value = 8000;
        t.description = "Novo Salário";
        transactionController.Update(t);
        Transaction checkTransaction = transactionController.GetById(t.id);

        Assert.Equal(checkTransaction.description, t.description);
        Assert.Equal(checkTransaction.value, t.value);

        category.name = "Dev Pleno";
        category.color = "#2B36B3";
        categoryController.Update(category);
        Category checkCategory = categoryController.GetById(category.id);

        Assert.Equal(checkCategory.name, category.name);
        Assert.Equal(checkCategory.color, category.color);

        expectedList.Remove(t);
        transactionController.Remove(t);
        retrievedList = transactionController.GetAllTransactionsOnly();
        Assert.Equal(retrievedList, expectedList);

        expectedListCat.Remove(category);
        categoryController.Remove(category);
        retrievedListCat = categoryController.GetAll();
        Assert.Equal(retrievedListCat, expectedListCat);
    }

    
}