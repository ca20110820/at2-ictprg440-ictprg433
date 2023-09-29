using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT2
{
    public class Contractor
    {
        private string uid;
        private string firstName;
        private string lastName;
        private double hourlyWage;

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new ArgumentException("First Name cannot be an empty string!");
                }
                else
                {
                    firstName = value;
                }
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new ArgumentException("Last Name cannot be an empty string!");
                }
                else
                {
                    lastName = value;
                }
            }
        }
        public DateTime? StartDate { get; set; }
        public double HourlyWage 
        {
            get
            {
                return hourlyWage;
            }
            set
            {
                if (value > 0)
                {
                    hourlyWage = value;
                }
                else
                {
                    throw new InvalidOperationException("Hourly Wage cannot be less than or equal to zero!");
                }
            }
        }
        public string ID
        {
            get { return uid; }
        }
        public string FullName
        {
            get
            {
                return $"{firstName} {lastName}";
            }
        }
        public bool IsAvailable
        {
            get
            {
                if (StartDate == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Contractor(string uid, string firstName, string lastName, double hourlyWage)
        {
            // --- Catch Invalid Arguments ---
            if (uid.Trim().Length == 0 || firstName.Trim().Length == 0 || lastName.Trim().Length == 0)
            {
                throw new ArgumentException("ID, First, and Last Names Cannot be empty strings!");
            }
            if (hourlyWage <= 0)
            {
                throw new ArgumentException("Hourly Wage cannot be less than or equal to zero!");
            }

            this.uid = uid;  // Custom UID
            this.firstName = firstName;
            this.lastName = lastName;
            this.hourlyWage = hourlyWage;
            StartDate = null;  // StartDate is null by default when instantiation
        }
        public Contractor(string firstName, string lastName, double hourlyWage)
        {
            // --- Catch Invalid Arguments ---
            if (hourlyWage <= 0)
            {
                throw new ArgumentException("Hourly Wage cannot be less than or equal to zero!");
            }

            // Generate Random UID
            Guid randomGuid = Guid.NewGuid();
            string randomId = Convert.ToBase64String(randomGuid.ToByteArray());
            randomId = randomId.Replace("/", "").Replace("+", "");
            randomId = randomId[..5];

            this.uid = randomId;  // Custom UID
            this.firstName = firstName;
            this.lastName = lastName;
            this.hourlyWage = hourlyWage;
            StartDate = null;  // StartDate is null by default when instantiation
        }

        public override string ToString()
        {
            string availability = IsAvailable ? "Available" : "Not Avilable";
            return $"({ID}) - {FullName} | ${HourlyWage} | {availability}";
        }
    }
}
