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
using System.Diagnostics;
using ControleFinanceiro.views;
using System.Globalization;
using System.Threading;
using ControleFinanceiro.controllers;

namespace ControleFinanceiro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Context context;
        private readonly TransactionController _transactionController;
        public Decimal balance { get; set; }
        public Decimal monthBalance { get; set; }

        public void updateBalanceText()
        {
            this.balance = _transactionController.GetBalance();
            SaldoText.Text = $"${string.Format("{0:0.00}", Convert.ToDecimal(this.balance))}";
            this.monthBalance = _transactionController.GetBalanceMonth();
            SaldoMesText.Text = $"${string.Format("{0:0.00}", Convert.ToDecimal(this.monthBalance))}";

            var redBrush = (Brush)new BrushConverter().ConvertFromString("#FFFF0000");
            var greenBrush = (Brush)new BrushConverter().ConvertFromString("#FF008000");
            SaldoText.Foreground = this.balance >= 0 ? greenBrush : redBrush;

            SaldoMesText.Foreground = this.monthBalance >= 0 ? greenBrush : redBrush;
        }
        public MainWindow(Context context)
        {
            this.context = context;
            _transactionController = new TransactionController(context);
            CultureInfo culture = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;
            
            InitializeComponent();
            DataContext = new HomeView(context);
            
            updateBalanceText();
        }
        private void HomeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new HomeView(context);
        }

        private void CategoryIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new CategoryView(context);
        }

        private void OutcomeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new OutcomeView(context);
        }

        private void IncomeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new IncomeView(context);
        }

        private void ChartIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new Chart(context);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if(text == "")
            {
                SaldoSeparator2.Visibility = Visibility.Collapsed;
                SaldoRestanteStack.Visibility = Visibility.Collapsed;
            }
            else
            {
                SaldoSeparator2.Visibility = Visibility.Visible;
                SaldoRestanteStack.Visibility = Visibility.Visible;

                var saldoRestante = this.monthBalance - Convert.ToDecimal(MetaTextBox.Text);
                SaldoRestanteText.Text = $"${string.Format("{0:0.00}", Convert.ToDecimal(saldoRestante))}";

                var redBrush = (Brush)new BrushConverter().ConvertFromString("#FFFF0000");
                var greenBrush = (Brush)new BrushConverter().ConvertFromString("#FF008000");
                SaldoRestanteText.Foreground = saldoRestante >= 0 ? greenBrush : redBrush;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string str = ((TextBox)sender).Text + e.Text;
            decimal i;
            e.Handled = !decimal.TryParse(str, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out i);
        }
    }
}
