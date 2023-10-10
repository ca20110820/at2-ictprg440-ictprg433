using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT2
{
    /// <summary>
    /// Represents the Contractor object.
    /// </summary>
    public class Contractor
    {
        private string uid;
        private string firstName;
        private string lastName;
        private double hourlyWage;

        /// <summary>
        /// Contractor's First Name.
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = ValidateName(value);
            }
        }
        
        /// <summary>
        /// Contractor's Last Name.
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = ValidateName(value);
            }
        }
        
        /// <summary>
        /// Contractor's Start Date for the Job, if assigned. 
        /// Default to null in initialisation.
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Contractor's Hourly Wage.
        /// </summary>
        public double HourlyWage 
        {
            get
            {
                return hourlyWage;
            }
            set
            {
                hourlyWage = ValidateHourlyWage(value);
            }
        }
        
        /// <summary>
        /// Contractor's Unique ID.
        /// </summary>
        public string ID
        {
            get { return uid; }
            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new ArgumentException("ID Cannot be an empty string!");
                }
                uid = value.Trim();
            }
        }
        
        /// <summary>
        /// Contractor's Full Name (i.e. First Name and Last Name).
        /// </summary>
        public string FullName
        {
            get
            {
                return $"{firstName} {lastName}";
            }
        }

        /// <summary>
        /// Value is true if the Contractor is not assigned to a job (i.e. StartDate is null), otherwise false.
        /// Default to false in initialisation.
        /// </summary>
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

        /// <summary>
        /// String representation of a Contractor object.
        /// </summary>
        /// <returns>Returns a string with format "(ID) - FullName".</returns>
        public override string ToString()
        {
            return $"{ID} - {FullName}";
        }

        /// <summary>
        /// Comparing if two Contractor instances are the same, excluding the ID.
        /// It is possible to have two instances with same properties other than ID.
        /// </summary>
        /// <param name="otherItem"></param>
        /// <returns>Returns true if two Contractor instances have same properties, otherwise false.</returns>
        public override bool Equals(object otherItem)
        {
            if (!(otherItem is Contractor))
            {
                return false;
            }
            else
            {
                Contractor item = (Contractor)otherItem;
                bool equalConditions =
                    item.FullName==FullName && item.IsAvailable== IsAvailable && item.StartDate== StartDate && item.HourlyWage== HourlyWage;
                if(equalConditions )
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
        /// <returns>Returns a string representing the Contractor's ID.</returns>
        private static string GenerateID(int length = 5)
        {
            Guid randomGuid = Guid.NewGuid();
            string randomId = Convert.ToBase64String(randomGuid.ToByteArray());
            randomId = randomId.Replace("/", "").Replace("+", "");
            randomId = randomId[..length];
            return randomId;
        }

        private string ValidateName(string inpName)
        {
            if (inpName.Trim().Length == 0)
            {
                throw new ArgumentException("First or Last Name cannot be an empty string!");
            }
            else
            {
                return inpName.Trim();
            }
        }
        private double ValidateHourlyWage(double inpHourlyWage)
        {
            if (inpHourlyWage <= 0)
            {
                throw new ArgumentException("Hourly Wage cannot be less than or equal to zero!");
            }
            else
            {
                return inpHourlyWage;
            }
        }
    }
}
