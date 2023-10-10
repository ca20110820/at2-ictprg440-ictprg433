using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Formats.Asn1.AsnWriter;

namespace AT2
{
    /// <summary>
    /// Recruitment Management Tracker.
    /// </summary>
    public class RecruitmentSystem
    {
        private List<Contractor> contractors = new();
        private List<Job> jobs = new();

        /// <summary>
        /// Gets the List of all the Contractors in the Recruitment System.
        /// </summary>
        public List<Contractor> Contractors { get { return contractors; } }

        /// <summary>
        /// Gets the List of all the Jobs in the Recruitment System.
        /// </summary>
        public List<Job> Jobs { get {  return jobs; } }

        /// <summary>
        /// Adds a New Contractor to the Recruitment System.
        /// </summary>
        /// <param name="contractor"></param>
        /// <exception cref="Exception"></exception>
        public void AddContractor(Contractor contractor)
        {
            IEnumerable<Contractor> ids = 
                from c in contractors
                where c.ID == contractor.ID
                select c;

            if(ids.Count() > 0)
            {
                throw new Exception($"{contractor.FullName} already exist with ID={contractor.ID}");
            }
            contractors.Add(contractor);
        }

        /// <summary>
        /// Removes an existing Contractor in the Recruitment System.
        /// </summary>
        /// <param name="contractor"></param>
        public void RemoveContractor(Contractor contractor)
        {
            if (!contractor.IsAvailable)  // Contractor is currently working and not available
            {
                //throw new Exception($"We cannot remove {contractor}! He/She is currently working.");
                // Find the Job where the Contractor is currently working
                // Desassign the Contractor from the Job
                foreach (Job job in jobs)
                {
                    if (job.ContractorAssigned == null)
                    {
                        continue;
                    }
                    else 
                    {
                        if (job.ContractorAssigned.ID == contractor.ID)
                        {
                            job.DeassignContractor();
                            break;
                        }
                    }
                }
            }
            contractors.Remove(contractor);  // Remove from the list
        }

        /// <summary>
        /// Adds a New Job in the Recruitment System.
        /// </summary>
        /// <param name="job"></param>
        /// <exception cref="Exception"></exception>
        public void AddJob(Job job)
        {
            IEnumerable<Job> ids =
            from j in jobs
                where j.ID == job.ID
                select j;

            if (ids.Any())
            {
                throw new Exception($"{job.Title} already exist with ID={job.ID}");
            }

            jobs.Add(job);
        }

        /// <summary>
        /// Removes an existing Job in the Recruitment System.
        /// </summary>
        /// <remarks>
        /// Allowed to remove jobs regardless of Completed Status and if there is a Contractor Assigned.
        /// </remarks>
        /// <param name="job"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveJob(Job job)
        {
            // Note: Optional, Not part of requirements.
            if (!job.Completed && job.ContractorAssigned != null)
            {
                job.DeassignContractor();
                jobs.Remove(job);
            }
            else if (job.Completed || (!job.Completed && job.ContractorAssigned == null))
            {
                jobs.Remove(job);
            }
            else
            {
                throw new Exception("Case that we have not considered");
            }

            
        }

        /// <summary>
        /// Assigns a Job with a Contractor.
        /// </summary>
        /// <remarks>
        /// Allowed to assign any job with a contractor, as long as the contractor to-be-assigned is available (i.e. not currently working). 
        /// Otherwise, a MessageBox will appear.
        /// </remarks>
        /// <param name="job"></param>
        /// <param name="contractor"></param>
        public void AssignJob(Job job, Contractor contractor)
        {
            job.AssignContractor(contractor);
        }

        /// <summary>
        /// Change the Status of Job to Completed.
        /// </summary>
        /// <remarks>
        /// Job's that are allowed to be updated to Completed status are the jobs with an assigned contractor.
        /// </remarks>
        /// <param name="job"></param>
        public void CompleteJob(Job job)
        {
            job.JobDone();
        }

        /// <summary>
        /// Gets all the Available Contractors.
        /// </summary>
        /// <returns>Returns a List of Contractors.</returns>
        public List <Contractor> GetAvailableContractors()
        {
            return contractors.Where(x => x.IsAvailable).ToList();
        }

        /// <summary>
        /// Gets all Unassigned Jobs.
        /// </summary>
        /// <returns>Returns a List of Jobs</returns>
        public List<Job> GetUnassignedJobs()
        {
            return jobs.Where(x => !x.Completed && x.ContractorAssigned == null).ToList();
        }

        /// <summary>
        /// Gets all Assigned Jobs.
        /// </summary>
        /// <returns>Returns a List of Jobs.</returns>
        public List<Job> GetAssignedJobs()
        {
            return jobs.Where(x => !x.Completed && x.ContractorAssigned is Contractor).ToList();
        }

        /// <summary>
        /// Gets all Jobs with Cost between the given Minimum and Maximum values.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns>Returns a List of Jobs.</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Job> GetJobByCost(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("Minimum Value cannot be greater than Maximum Value!");
            }
            if (minValue < 0 || maxValue < 0)
            {
                throw new ArgumentException("Minimum or Maximum values cannot be negative!");
            }
            return jobs.Where(x => minValue <= x.Cost && x.Cost <= maxValue).ToList();
        }

    }
}
