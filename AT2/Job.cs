using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT2
{
    public class Job
    {
        private string uid;
        private string title;
        private double cost;
        private bool completed = false;
        private Contractor? contractorAssigned = null;
        
        public string ID
        {
            get 
            { 
                return uid; 
            }
            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new Exception("Job ID cannot be an empty string!");
                }
                uid = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set 
            { 
                if(value.Trim().Length == 0)
                {
                    throw new Exception("Title cannot be an empty string!");
                }
                title = value;
            }
        }

        public DateTime Date { get; set; }

        public double Cost
        {
            get
            {
                return cost;
            }
            set
            {
                if (value  > 0)
                {
                    cost = value;
                }
                else
                {
                    throw new Exception("Cost cannot be less than or equal to zero!");
                }
            }
        }

        public Contractor? ContractorAssigned
        {
            get
            {
                return contractorAssigned;
            }
            set
            {
                if (completed)
                {
                    contractorAssigned = null;
                }

                if (contractorAssigned == null)
                {
                    if (value == null)
                    {
                    }
                    else  // i.e. Assigning a New Contractor
                    {
                        Debug.Assert(value.IsAvailable, $"{value.FullName} is Working!");  // Assume that the New Contractor is Available, otherwise throw error
                        value.StartDate = Date;  // Update StartDate of Contractor object
                        contractorAssigned = value;
                    }
                }
                else  // contractorAssigned not null
                {
                    if (value == null)  // i.e. Deassigning the Current Contractor
                    {
                        contractorAssigned.StartDate = null;  // Reset Contractor object StartDate back to null
                        contractorAssigned = null;  // Set contractorAssigned to null
                    }
                    else  // i.e. We are replacing the Current Contractor with a New One
                    {
                        Debug.Assert(value.IsAvailable, $"{value.FullName} is Working!");  // Assume that the New Contractor is Available, otherwise throw error
                        contractorAssigned.StartDate = null;  // Reset Old Contractor object StartDate back to null
                        value.StartDate = Date;  // Update StartDate of New Contractor object
                        contractorAssigned = value;  // Set contractorAssigned to the New Contractor object
                    }
                }
            }
        }

        public bool Completed
        {
            get
            {
                return completed;
            }
            set
            {
                if (!completed && value)  // i.e. Job is Now Completed
                {
                    Debug.Assert(contractorAssigned != null, "Cannot complete a job without any assigned contractor!");

                    // Update Properties of Contractor
                    contractorAssigned.StartDate = null;  // Make the Contractor Available
                    contractorAssigned = null;  // Set contractorAssigned to null

                    // Set completed to true
                    completed = true;
                }
                if(completed && !value)  // i.e. Trying to re-open a unique job that is already archived, throw error
                {
                    throw new Exception("Cannot Re-open a Completed Job!");
                }
            }
        }

        public Job(string id, string title, DateTime date, double cost)
        {
            ID = id;
            Title = title;
            Date = date;
            Cost = cost;
        }
        public Job(string id, string title, DateTime date, double cost, Contractor contractor)
        {
            ID = id;
            Title = title;
            Date = date;
            Cost = cost;
            ContractorAssigned = contractor;
        }
        public Job(string title, DateTime date, double cost)
        {
            ID = GenerateID();
            Title = title;
            Date = date;
            Cost = cost;
        }
        public Job(string title, DateTime date, double cost, Contractor contractor)
        {
            ID = GenerateID();
            Title = title;
            Date = date;
            Cost = cost;
            ContractorAssigned = contractor;
        }

        public override string ToString()
        {
            string contractorName = ContractorAssigned == null ? "No Contractor" : ContractorAssigned.FullName;
            string status = Completed ? "Completed" : "Not Complete";
            return $"({ID}) - {Title} (status) | {Date.Date.ToString("yyyy-MM-dd")} | ${Cost} | {contractorName}";
        }

        private static string GenerateID(int length = 5)
        {
            Guid randomGuid = Guid.NewGuid();
            string randomId = Convert.ToBase64String(randomGuid.ToByteArray());
            randomId = randomId.Replace("/", "").Replace("+", "");
            randomId = randomId[..length];
            return randomId;
        }
    }
}
