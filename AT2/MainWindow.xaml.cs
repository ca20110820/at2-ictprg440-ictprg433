using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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

            if (selectedContractor == null)  // Scenario of mis-selection
            {
                tabctrlDataGrids.SelectedItem = tabitemContractor;  // Focus on Contractor Tab Item
                datagridContractor.ItemsSource = null;
                datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
                return;
            }

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

            if (selectedJob == null)  // Scenario of mis-selection
            {
                tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
                datagridJob.ItemsSource = null;
                datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
                return;
            }

            comboboxContractorAssigned.ItemsSource = recruitmentSystem.GetContractors();

            // Update and Change the Forms in Job Group
            txtbxTitle.Text = selectedJob.Title;
            datepickerDate.SelectedDate = selectedJob.Date;
            txtbxCost.Text = selectedJob.Cost.ToString("0.##");
            comboboxCompleted.SelectedIndex = selectedJob.Completed ? 0 : 1;  // 0->"Completed" and 1-> "Not Complete"
            comboboxContractorAssigned.SelectedItem = selectedJob.ContractorAssigned != null ? selectedJob.ContractorAssigned : null;
        }

        private void btnAddContractor_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Contractor newContractor = new Contractor(txtbxFirstName.Text, txtbxLastName.Text, double.Parse(txtbxHourlyWage.Text));  // Instantiate New Contractor
                recruitmentSystem.AddContractor(newContractor);  // Add new Contractor
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }
            tabctrlDataGrids.SelectedItem = tabitemContractor;  // Focus on Contractor Tab Item
            datagridContractor.ItemsSource = null;
            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
        }

        private void btnRemoveContractor_Click(object sender, RoutedEventArgs e)
        {
            Contractor selectedContractor = (Contractor)datagridContractor.SelectedItem;

            if (selectedContractor == null)  // Warn user if no selection
            {
                MessageBox.Show("Please Select a Contractor to Remove", "Warn");
                tabctrlDataGrids.SelectedItem = tabitemContractor;  // Focus on Contractor Tab Item
                return;
            }

            try
            {
                recruitmentSystem.RemoveContractor(selectedContractor);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }

            datagridJob.ItemsSource = null;
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
            datagridContractor.ItemsSource = null;
            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
        }

        private void btnUpdateContractor_Click(object sender, RoutedEventArgs e)
        {
            Contractor selectedContractor = (Contractor)datagridContractor.SelectedItem;  // Get Selected Contractor

            if (selectedContractor == null)  // Warn user if no selection
            {
                MessageBox.Show("Please Select a Contractor to Update", "Warn");
                tabctrlDataGrids.SelectedItem = tabitemContractor;  // Focus on Contractor Tab Item
                return;
            }

            try
            {
                selectedContractor.FirstName = txtbxFirstName.Text;
                selectedContractor.LastName = txtbxLastName.Text;
                //selectedContractor.StartDate = datepickerStartDate.SelectedDate;
                selectedContractor.HourlyWage = Math.Round(double.Parse(txtbxHourlyWage.Text), 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }

            datagridJob.ItemsSource = null;
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
            datagridContractor.ItemsSource = null;
            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
        }

        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Job newJob = new Job(txtbxTitle.Text, datepickerDate.SelectedDate.Value, double.Parse(txtbxCost.Text));
                recruitmentSystem.AddJob(newJob);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }
            tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
            datagridJob.ItemsSource = null;
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
        }

        private void btnRemoveJob_Click(object sender, RoutedEventArgs e)
        {
            Job selectedJob = (Job)datagridJob.SelectedItem;

            if (selectedJob == null)  // Warn user if no selection
            {
                MessageBox.Show("Please Select a Job to Remove", "Warn");
                tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
                return;
            }

            try
            {
                recruitmentSystem.RemoveJob(selectedJob);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }

            datagridContractor.ItemsSource = null;
            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
            datagridJob.ItemsSource = null;
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
        }

        private void btnUpdateJob_Click(object sender, RoutedEventArgs e)
        {
            Job selectedJob = (Job)datagridJob.SelectedItem;  // Get Selected Job

            if (selectedJob == null)  // Warn user if no selection
            {
                MessageBox.Show("Please Select a Job to Update", "Warn");
                tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
                return;
            }

            try
            {
                // Only Perform Changes on "Basic" Properties. `Completed` and `ContractorAssigned` will be 
                // implemented in different event-handlers.
                selectedJob.Title = txtbxTitle.Text;
                selectedJob.Date = datepickerDate.SelectedDate.Value;
                selectedJob.Cost = Math.Round(double.Parse(txtbxCost.Text), 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }

            datagridContractor.ItemsSource = null;
            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
            datagridJob.ItemsSource = null;
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
        }

        private void comboboxCompleted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Index Changed - Current State
            int selectedIndex = comboboxCompleted.SelectedIndex;  // 0->"Completed" and 1-> "Not Complete"

            Job selectedJob = (Job)datagridJob.SelectedItem;  // Get Selected Job, if selected (could be null)

            if (selectedJob is null)
            {
                tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
                MessageBox.Show("Please Select a Job", "Warn");
                return;
            }

            if (selectedIndex == 0)  // Job Completed (from 1 to 0)
            {
                try
                {
                    recruitmentSystem.CompleteJob(selectedJob);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Error");
                    return;
                }
            }

            datagridContractor.ItemsSource = null;
            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
            datagridJob.ItemsSource = null;
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
        }

        private void comboboxContractorAssigned_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contractor selectedContractor = (Contractor)comboboxContractorAssigned.SelectedItem;  // Changed ContractorAssigned - Current State
            Job selectedJob = (Job)datagridJob.SelectedItem;  // Get Selected Job

            if (selectedJob is null)  // Check if no Job selected
            {
                return;
            }
            if (selectedJob.ContractorAssigned == selectedContractor)
            {
                return;
            }

            try
            {
                recruitmentSystem.AssignJob(selectedJob, selectedContractor);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return;
            }

            datagridContractor.ItemsSource = null;
            datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
            datagridJob.ItemsSource = null;
            datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
        }

        private void comboboxFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = comboboxFilters.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:  // Empty
                    // Back to Basic View
                    datagridJob.ItemsSource = null;
                    datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
                    datagridContractor.ItemsSource = null;
                    datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
                    tabctrlDataGrids.SelectedItem = tabitemContractor;  // Focus on Contractor Tab Item
                    return; 
                case 1:  // Get All Contractors
                    datagridContractor.ItemsSource = null;
                    datagridContractor.ItemsSource = recruitmentSystem.GetContractors();  // Update the Contractor DataGrid
                    tabctrlDataGrids.SelectedItem = tabitemContractor;  // Focus on Contractor Tab Item
                    return;
                case 2:  // Get All Jobs
                    datagridJob.ItemsSource = null;
                    datagridJob.ItemsSource = recruitmentSystem.GetJobs();  // Update the Job DataGrid
                    tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
                    return;
                case 3:  // Get Available Contractors
                    datagridContractor.ItemsSource = null;
                    datagridContractor.ItemsSource = recruitmentSystem.GetAvailableContractors();  // Update the Contractor DataGrid
                    tabctrlDataGrids.SelectedItem = tabitemContractor;  // Focus on Contractor Tab Item
                    return;
                case 4:  // Get Unassigned Jobs
                    datagridJob.ItemsSource = null;
                    datagridJob.ItemsSource = recruitmentSystem.GetUnassignedJobs();  // Update the Job DataGrid
                    tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
                    return;
                case 5:  // Get Jobs By Cost
                    try
                    {
                        datagridJob.ItemsSource = null;
                        datagridJob.ItemsSource = recruitmentSystem.GetJobByCost(double.Parse(txtMinValue.Text), double.Parse(txtMaxValue.Text));  // Update the Job DataGrid
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message, "Error");
                    }
                    finally
                    {
                        tabctrlDataGrids.SelectedItem = tabitemJob;  // Focus on Job Tab Item
                    }
                    return;

            }
        }
    }
}
