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

namespace AT2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        RecruitmentSystem recruitmentSystem = new();

        public MainWindow()
        {
            InitializeComponent();

            recruitmentSystem.AddContractor(new Contractor("Cedric", "Anover", 50));
            recruitmentSystem.AddContractor(new Contractor("John", "Cena", 25));
            recruitmentSystem.AddContractor(new Contractor("Jack", "Ma", 75));
            recruitmentSystem.AddContractor(new Contractor("Elon", "Musk", 70));
            recruitmentSystem.AddContractor(new Contractor("Bill", "Gates", 80));
            recruitmentSystem.AddContractor(new Contractor("John", "Wick", 19));

            recruitmentSystem.AddJob(new Job("Data Engineer", new DateTime(2023, 11, 5), 300000));
            recruitmentSystem.AddJob(new Job("Network Engineer", new DateTime(2023, 10, 6), 96000));
            recruitmentSystem.AddJob(new Job("Network Programmer", new DateTime(2023, 12, 15), 82000));
            recruitmentSystem.AddJob(new Job("Data Scientist", new DateTime(2024, 1, 5), 100000));
            recruitmentSystem.AddJob(new Job("Data Scientist", new DateTime(2024, 1, 5), 100000));
            recruitmentSystem.AddJob(new Job("DevOps Specialist", new DateTime(2024, 1, 16), 90000));

            recruitmentSystem.AssignJob(recruitmentSystem.GetJobs().ToArray()[0], recruitmentSystem.GetContractors().ToArray()[0]);
            recruitmentSystem.AssignJob(recruitmentSystem.GetJobs().ToArray()[1], recruitmentSystem.GetContractors().ToArray()[1]);

            recruitmentSystem.CompleteJob(recruitmentSystem.GetJobs().ToArray()[1]);

            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();
        }

        private void datagridContractor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contractor selectedContractor = (Contractor)datagridContractor.SelectedItem;

            // Update and Change the Forms in Contractor Group
            txtbxFirstName.Text = selectedContractor.FirstName;
            txtbxLastName.Text = selectedContractor.LastName;
            datepickerStartDate.SelectedDate = selectedContractor.StartDate;
            txtbxHourlyWage.Text = selectedContractor.HourlyWage.ToString("0.##");
            labelAvailability.Content = selectedContractor.IsAvailable ? "Status: Available" : "Status: Not Available";
        }

        private void datagridJob_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Job selectedJob = (Job)datagridJob.SelectedItem;

            comboboxContractorAssigned.ItemsSource = recruitmentSystem.GetContractors();

            // Update and Change the Forms in Job Group
            txtbxTitle.Text = selectedJob.Title;
            datepickerDate.SelectedDate = selectedJob.Date;
            txtbxCost.Text = selectedJob.Cost.ToString("0.##");
            comboboxCompleted.SelectedIndex = selectedJob.Completed ? 0 : 1;  // 0->"Completed" and 1-> "Not Complete"
            comboboxContractorAssigned.SelectedItem = selectedJob.ContractorAssigned != null ? selectedJob.ContractorAssigned : null;
        }
    }
}
