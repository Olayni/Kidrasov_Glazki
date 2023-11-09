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

namespace Kidrasov_Glazki_save
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            var currentServices = Kidrasov_glazkiEntities.GetContext().Agent.ToList();

            AgentListView.ItemsSource = currentServices;
            ComboFilter.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();   
        }

        public void UpdateServices()
        {

            var currentServices = Kidrasov_glazkiEntities.GetContext().Agent.ToList();

            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeString == "ЗАО")).ToList();
            }

            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (p.AgentTypeString == "МКК")).ToList();
            }

            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "МФО").ToList();
            }

            if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ОАО").ToList();
            }

            if (ComboType.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ООО").ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ПАО").ToList();
            }



            if (ComboFilter.SelectedIndex == 1)
            {
                currentServices = currentServices.OrderBy(p => p.Title).ToList();
            }
            if (ComboFilter.SelectedIndex == 2)
            {
                currentServices = currentServices.OrderByDescending(p => p.Title).ToList();
            }

            if (ComboFilter.SelectedIndex == 3)
            {
                currentServices = currentServices.OrderBy(p => p.Title).ToList();
            }
            if (ComboFilter.SelectedIndex == 4)
            {
                currentServices = currentServices.OrderByDescending(p => p.Title).ToList();
            }

            if (ComboFilter.SelectedIndex == 5)
            {
                currentServices = currentServices.OrderBy(p => p.Priority).ToList();
            }
            if (ComboFilter.SelectedIndex == 6)
            {
                currentServices = currentServices.OrderByDescending(p => p.Priority).ToList();
            }
            currentServices = currentServices.Where(p => p.Title.ToLower().Contains(Search.Text.ToLower()) ||
            p.Phone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").ToLower().Contains(Search.Text.ToLower()) ||
            p.Email.ToLower().Contains(Search.Text.ToLower())).ToList();
            AgentListView.ItemsSource = currentServices;
        }

        private void Search_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }
    }
}
    



