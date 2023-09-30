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
        private DateTime date;
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
                title = value.Trim();
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value.Date <= DateTime.Now.Date)  // If the New Date Value is Older than Date Now, throw error
                {
                    throw new Exception($"Job Date cannot be equal or older than date now {DateTime.Now.Date.ToString("dd/MM/yyyy")}");
                }
                date = value;
                // Update ContractorAssigned, if exist
                if (ContractorAssigned != null)
                {
                    ContractorAssigned.StartDate = date;
                }
            }
        }

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
                        if (!value.IsAvailable)  // Assume that the New Contractor is Available, otherwise throw error
                        {
                            throw new Exception($"{value.FullName} is Working!");
                        }
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
                        if (!value.IsAvailable)  // Assume that the New Contractor is Available, otherwise throw error
                        {
                            throw new Exception($"{value.FullName} is Working!");
                        }
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
                    if (contractorAssigned == null)
                    {
                        throw new Exception("Cannot complete a job without any assigned contractor!");
                    }
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
            return $"{ID} - {Title}";
        }

        public override bool Equals(object otherItem)
        {
            if (!(otherItem is Job))
            {
                return false;
            }
            else
            {
                Job item = (Job)otherItem;
                bool equalConditions =
                    item.Title == Title && item.Date == Date && item.Cost == Cost && item.ContractorAssigned== ContractorAssigned && item.Completed == Completed;
                if (equalConditions)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
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
