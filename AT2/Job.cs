using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT2
{
    /// <summary>
    /// Represents the Job object.
    /// </summary>
    public class Job
    {
        private string uid;
        private string title;
        private DateTime date;
        private double cost;
        private bool completed = false;
        private Contractor? contractorAssigned = null;

        /// <summary>
        /// Job's Unique ID.
        /// </summary>
        public string ID
        {
            get 
            { 
                return uid; 
            }
            set
            {
                uid = ValidateString(value, "Invalid ID!");
            }
        }

        /// <summary>
        /// Job Title.
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set 
            {
                title = ValidateString(value, "Invalid Title!");
            }
        }

        /// <summary>
        /// Job's Start Date.
        /// This will be passed to the Contractor assigned to the job.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = ValidateDate(value);

                // Update ContractorAssigned, if exist
                if (ContractorAssigned != null)
                {
                    ContractorAssigned.StartDate = date;
                }
            }
        }

        /// <summary>
        /// Job's Cost.
        /// </summary>
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

        /// <summary>
        /// Represents the Contractor assigned to this Job.
        /// Default to null in initialisation.
        /// </summary>
        public Contractor? ContractorAssigned
        {
            get
            {
                return contractorAssigned;
            }
            set
            {
                if (completed)  // If job is already in a completed state, cannot assign a new contractor. Throw an error.
                {
                    throw new Exception("Cannot assign a contractor to a completed job!");
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

        /// <summary>
        /// Represents the current Status of the Job object. 
        /// Value is true if the job is completed, otherwise false.
        /// Default to false in initialisation.
        /// Once Completed is set to true, it can never revert back to false.
        /// </summary>
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

        /// <summary>
        /// String representation of a Job object. 
        /// </summary>
        /// <returns>Returns a string with format "(ID) - Title".</returns>
        public override string ToString()
        {
            return $"{ID} - {Title}";
        }

        /// <summary>
        /// Comparing if two Jobs instances are the same, excluding the ID.
        /// </summary>
        /// <param name="otherItem"></param>
        /// <returns>Returns true if two Job instances have same properties, otherwise false.</returns>
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

        /// <summary>
        /// Auxiliary method for generating random ID with a given length.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Returns a string representing the Job's ID.</returns>
        private static string GenerateID(int length = 5)
        {
            Guid randomGuid = Guid.NewGuid();
            string randomId = Convert.ToBase64String(randomGuid.ToByteArray());
            randomId = randomId.Replace("/", "").Replace("+", "");
            randomId = randomId[..length];
            return randomId;
        }

        private string ValidateString(string inpStr, string errorMsg)
        {
            if (inpStr.Trim().Length == 0)
            {
                throw new ArgumentException(errorMsg);
            }
            return inpStr.Trim();
        }

        private DateTime ValidateDate(DateTime inpDate)
        {
            if (inpDate.Date <= DateTime.Now.Date)  // If the New Date Value is Older than Date Now, throw error
            {
                throw new Exception($"Job Date cannot be equal or older than date now {DateTime.Now.Date.ToString("dd/MM/yyyy")}");
            }

            return inpDate;
        }
    }
}
