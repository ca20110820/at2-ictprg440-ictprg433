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
                    firstName = value.Trim();
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
                    lastName = value.Trim();
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
            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new ArgumentException("ID Cannot be an empty string!");
                }
                uid = value;
            }
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
            ID = uid;  // Custom UID
            FirstName = firstName;
            LastName = lastName;
            HourlyWage = hourlyWage;
            StartDate = null;  // StartDate is null by default when instantiation
        }
        public Contractor(string firstName, string lastName, double hourlyWage)
        {
            ID = GenerateID();  // Custom UID
            FirstName = firstName;
            LastName = lastName;
            HourlyWage = hourlyWage;
            StartDate = null;  // StartDate is null by default when instantiation
        }

        public override string ToString()
        {
            return $"{ID} - {FullName}";
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
