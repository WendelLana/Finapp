# Financial Control
Personal Finance Control Project with records of expenses (outflow) and income (inflow), along with category registration for quick identification of each record. It also includes charts for dynamic visualization of the system's records.

## Asked Requirements
* Use of SQLite database for data persistence.
* Utilization of layered architecture pattern, MVC.
* Creation of categories for expenses.
* Recording expenses with the following data: date, amount, description, and category.
* Viewing the total expenses per month.
* Viewing the total expenses per category for the month.
* Editing registered categories.
* Editing or deleting recorded expenses.
* Registering color and icon for categories.
* Pie chart for viewing expenses by category and month.
* Vertical bar chart for comparing expenses between months and categories.
* Line chart that allows viewing expenses by day.

## User Manual
### How to Register a Category
1. Click on “Categoria” to access the category interface.
2. Click on “Cadastrar”.
3. Fill out the form in the new window.
4. Click “Confirmar” to register the category.

### How to Edit a Category
1. Click on “Categoria” to access the category interface.
2. Click “Editar” on the record you want to modify.
3. In the new window, edit the necessary information.
4. Click “Confirmar” and your changes will be saved.

### How to Remove a Category
1. Click on “Categoria” to access the category interface.
2. Click “Remover” on the record you want to delete.

### How to Register an Income/Expense Record
1. Click on the section where you want to register a record (“Entrada” - Income or “Saída” - Expense).
2. Click the “Cadastrar” button in the upper right corner.
3. Fill out the information in the registration window.
4. Click “Confirmar” to register the record.

### How to Edit an Income/Expense Record
1. Click on the section where you want to edit a record (“Entrada” - Income or “Saída” - Expense).
2. Click the “Editar” button next to the record you want to edit.
3. Modify the information in the edit window.
4. Click “Confirmar” to save the changes.

### How to Remove an Income/Expense Record
1. Click on the section where you want to remove a record (“Entrada” - Income or “Saída” - Expense).
2. Click “Remover” on the record you want to delete.


### How to View the Charts
1. Click on “Gráficos” to access the charts interface.
2. Click on the type of chart you want to view:
  - Pie (“Pizza”): income (earnings) and expenses (spending) by category or month in the current year.
  - Bar (“Barra”): total expenses by category in each month of the current year.
  - Line (“Linha”): total expenses each day throughout the year.
3. Choose the desired filter, if necessary.
