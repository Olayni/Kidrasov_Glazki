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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        public void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords; ;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;
                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                AgentListView.ItemsSource = CurrentPageList;
                AgentListView.Items.Refresh();
            }

        }
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
            Manager.MainFrame.Navigate(new AddEditPage(null));
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

            AgentListView.ItemsSource = currentServices;
            TableList = currentServices;
            ChangePage(0, 0);
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Kidrasov_glazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = Kidrasov_glazkiEntities.GetContext().Agent.ToList();
            }
            UpdateServices();
        }

        private void Search_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void PageListBox_MouseUp(object sender, MouseEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }
        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }
        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }
    }
}
    



