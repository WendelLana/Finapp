using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControleFinanceiro.controllers;
using ControleFinanceiro.models;

namespace ControleFinanceiro.views
{
    /// <summary>
    /// Interação lógica para HomeView.xam
    /// </summary>
    public partial class HomeView : UserControl
    {
        private readonly TransactionController transactionController;
        private readonly CategoryController categoryController;
        public HomeView(Context context)
        {
            transactionController = new TransactionController(context);
            categoryController = new CategoryController(context);
            InitializeComponent();

            // Inicializa a Combo Box de filtros por categoria
            CategoryFilterCBox.SelectedValuePath = "Key";
            CategoryFilterCBox.DisplayMemberPath = "Value";
            CategoryFilterCBox.Items.Add(new KeyValuePair<Category, string>(null, "All Categories"));
            List<Category> categoriesList = categoryController.GetAll().OrderBy(c => c.name).ToList();
            foreach (var c in categoriesList)
            {
                CategoryFilterCBox.Items.Add(new KeyValuePair<Category, string>(c, c.name));
            }
            CategoryFilterCBox.SelectedIndex = 0;
        }

        // Inicializa a tabela baseada nos filtros (mesmo na primeira execução)
        private void UpdateFilters()
        {
            bool isRecurrentSelected = (TabControl.SelectedIndex == 1) ? true : false;

            List<Transaction> transactionsList;
            if (isRecurrentSelected)
                transactionsList = transactionController.GetAllRecurrent();
            else
                transactionsList = transactionController.GetAll();

            var dateStart = DateStart.Value;
            if (dateStart != null)
            {
                transactionsList = transactionsList.Where(t => t.date >= dateStart).ToList();
            }
            var dateEnd = DateEnd.Value;
            if (dateEnd != null)
            {
                transactionsList = transactionsList.Where(t => t.date <= dateEnd).ToList();
            }

            bool isSaidaChecked = SaidaFilterCBox.IsChecked ?? false;
            bool isEntradaChecked = EntradaFilterCBox.IsChecked ?? false;
            if (isEntradaChecked)
                transactionsList = transactionsList.Where(t => t.transactionType == "I").ToList();
            else if (isSaidaChecked)
                transactionsList = transactionsList.Where(t => t.transactionType == "O").ToList();

            KeyValuePair<Category, string> currentPair = (KeyValuePair<Category, string>)CategoryFilterCBox.SelectedItem;
            if (currentPair.Key != null)
                transactionsList = transactionsList.Where(t => t.Category == currentPair.Key).ToList();

            if (isRecurrentSelected)
            {
                RecurrentHomeTable.ItemsSource = transactionsList.OrderByDescending(i => i.date.Date)
                    .ThenByDescending(i => i.date.TimeOfDay).Take(20).ToList();
            }
            else
            {
                HomeTable.ItemsSource = transactionsList.OrderByDescending(i => i.date.Date)
                    .ThenByDescending(i => i.date.TimeOfDay).Take(20).ToList();
            }
        }

        private void TabChanged(object sender, RoutedEventArgs e)
        {
            bool isRecurrentSelected = (TabControl.SelectedIndex == 1) ? true : false;
            
            if(isRecurrentSelected)
            {
                DateStart.FormatString = "MM / yyyy";
                DateEnd.FormatString = "MM / yyyy";
            }
            else
            {
                DateStart.FormatString = "dd/MM/yyyy HH:mm tt";
                DateEnd.FormatString = "dd/MM/yyyy HH:mm tt";
            }
            
            UpdateFilters();
        }

        private void EntradaCheckBoxClick(object sender, RoutedEventArgs e)
        {
            bool isSaidaChecked = SaidaFilterCBox.IsChecked ?? false;
            if (isSaidaChecked)
                SaidaFilterCBox.IsChecked = false;
            UpdateFilters();
        }

        private void SaidaCheckBoxClick(object sender, RoutedEventArgs e)
        {
            bool isEntradaChecked = EntradaFilterCBox.IsChecked ?? false;
            if (isEntradaChecked)
                EntradaFilterCBox.IsChecked = false;
            UpdateFilters();
        }

        private void CategoryFilterChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilters();
        }

        private void DateChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilters();
        }
    }
}
