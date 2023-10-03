﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Formats.Asn1.AsnWriter;

namespace AT2
{
    public class RecruitmentSystem
    {
        private List<Contractor> contractors = new();
        private List<Job> jobs = new();

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

        public void RemoveContractor(Contractor contractor)
        {
            if (!contractor.IsAvailable)  // Contractor is currently working and not available
            {
                //throw new Exception($"We cannot remove {contractor}! He/She is currently working.");
                // Find the Job where the Contractor is currently working
                // Desassign the Contractor from the Job
                foreach (Job job in jobs)
                {
                    if (job == null)
                    {
                        return;
                    }
                    else 
                    {
                        if (job.ContractorAssigned.ID == contractor.ID)
                        {
                            job.ContractorAssigned = null;  // Deassign the Contractor from the job
                            break;
                        }
                    }
                }
            }
            contractors.Remove(contractor);  // Remove from the list
        }

        public void AddJob(Job job)
        {
            IEnumerable<Job> ids =
            from j in jobs
                where j.ID == job.ID
                select j;

            if (ids.Count() > 0)
            {
                throw new Exception($"{job.Title} already exist with ID={job.ID}");
            }

            jobs.Add(job);
        }

        public void RemoveJob(Job job)
        {
            // Note: Optional, Not part of requirements.
            if (!job.Completed && job.ContractorAssigned != null)
            {
                job.ContractorAssigned = null;  // Reset to null to Update Contractor
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

        public void AssignJob(Job job, Contractor contractor)
        {
            job.ContractorAssigned = contractor;
        }

        public void CompleteJob(Job job)
        {
            job.Completed = true;
        }

        public List<Contractor> GetContractors()
        {
            return contractors;
        }

        public List<Job> GetJobs()
        {
            return jobs;
        }

        public List <Contractor> GetAvailableContractors()
        {
            return contractors.Where(x => x.IsAvailable).ToList();
        }

        public List<Job> GetUnassignedJobs()
        {
            return jobs.Where(x => !x.Completed && x.ContractorAssigned == null).ToList();
        }

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
