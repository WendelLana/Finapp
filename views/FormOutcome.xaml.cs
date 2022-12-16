﻿using System;
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
using System.Windows.Shapes;
using ControleFinanceiro.models;
using ControleFinanceiro.controllers;
using MahApps.Metro.IconPacks;
using System.Text.RegularExpressions;

namespace ControleFinanceiro.views
{
    /// <summary>
    /// Interaction logic for GerenciarGasto.xaml
    /// </summary>
    public partial class FormOutcome : Window
    {

        private string typeAction = "";
        private OutcomeView parentView;
        private OutcomeController _controller;
        public FormOutcome()
        {
            parentView = (OutcomeView)Application.Current.MainWindow.DataContext;
            InitializeComponent();
            
        }

        public FormOutcome(string titleLabel, Transaction data, string type)
        {
            parentView = (OutcomeView)Application.Current.MainWindow.DataContext;
            typeAction = type;

            InitializeComponent();
            _controller = parentView.GetOutcomeController();
            TitleLabel.Content = titleLabel;
            OutcomeGrid.DataContext = data;
            if (data.recorrente)
                RecorrenteCBox.IsChecked = true;

            var availablesCategories = _controller.GetAvailableCategories();

            availablesCategories.ToList().ForEach(val =>
            {
                var comboBoxItem = new ComboBoxItem();
                var itemContent = new StackPanel { Orientation = Orientation.Horizontal };

                PackIconBase icon;

                switch (val.pack)
                {
                    case "PackIconMaterial":
                        icon = new PackIconMaterial()
                        {
                            Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), val.icon),
                            Width = 25,
                            Height = 25,
                            Foreground = (Brush)new BrushConverter().ConvertFromString(val.color),
                        };
                        break;
                    default: icon = new PackIconMaterial(); break;
                }

                itemContent.Children.Add(icon);
                itemContent.Children.Add(new Label { Content = val.name, Margin = new Thickness(20, 0, 0, 0) });
                comboBoxItem.Content = itemContent;
                CategoriesComboBox.Items.Add(comboBoxItem);

                if (type == "editar")
                {
                    var index = availablesCategories.FindIndex(i => i.id == data.categoryId);
                    CategoriesComboBox.SelectedIndex = index;
                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = CategoriesComboBox.SelectedIndex;
            if (selectedIndex == -1 || Value.Text.Length == 0)
            {
                MessageBox.Show("Por favor, selecione uma categoria e coloque um valor válido!", "Alerta", MessageBoxButton.OK);
                return;
            }
            var selectedCategory = _controller.GetAvailableCategories()[selectedIndex];

            if (typeAction.Equals("cadastrar"))
            {
                Transaction newOutcome = OutcomeGrid.DataContext as Transaction;
                newOutcome = OutcomeGrid.DataContext as Transaction;
                newOutcome.categoryId = selectedCategory.id;
                newOutcome.recorrente = RecorrenteCBox.IsChecked ?? false;
                parentView.AddOutcome(newOutcome);
            }
            else if (typeAction.Equals("editar"))
            {
                Transaction editOutcome = OutcomeGrid.DataContext as Transaction;
                editOutcome = OutcomeGrid.DataContext as Transaction;
                editOutcome.categoryId = selectedCategory.id;
                editOutcome.recorrente = RecorrenteCBox.IsChecked ?? false;
                parentView.EditOutcome(editOutcome);
            }
            else
            {
                //error
            }
            Close();
        }

        private void Recorrente_Click(object sender, RoutedEventArgs e)
        {
            bool recorrenteClicked = RecorrenteCBox.IsChecked ?? false;
            if(recorrenteClicked)
            {
                DatePicker.FormatString = "MM / yyyy";
                DateLabel.Content = "Mês de Início";
            }
            else
            {
                DatePicker.FormatString = "dd/MM/yyyy HH:mm tt";
                DateLabel.Content = "Data";
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text) || (Value.Text.Contains('.') && e.Text == ".");
        }
    }
}
