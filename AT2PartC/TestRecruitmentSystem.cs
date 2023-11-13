using AT2;

namespace AT2PartC
{
    [TestClass]
    public class TestRecruitmentSystem
    {
        private RecruitmentSystem recruitmentSystem = new();

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            recruitmentSystem = new();
            recruitmentSystem.AddContractor(new Contractor("Cedric", "Anover", 50));
            recruitmentSystem.AddContractor(new Contractor("David", "Hilbert", 65));
            recruitmentSystem.AddContractor(new Contractor("Terence", "Tao", 100));

            recruitmentSystem.AddJob(new Job("Mathematician", new DateTime(2024, 2, 24), 250000));
            recruitmentSystem.AddJob(new Job("Maths Professor", new DateTime(2024, 1, 18), 300000));
            recruitmentSystem.AddJob(new Job("Algorithmic Trader", new DateTime(2024, 6, 2), 500000));
        }

        [TestMethod]
        public void AddContractor_InvalidNames()
        {
            // Arrrange
            recruitmentSystem = new();

            // Act and Assert
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddContractor(new Contractor("", "Tao", 100)));
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddContractor(new Contractor("  ", "Tao", 100)));
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddContractor(new Contractor("Terence", "", 100)));
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddContractor(new Contractor("Terence", "  ", 100)));
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddContractor(new Contractor(" ", "  ", 100)));
        }

        [TestMethod]
        public void AddContractor_SpacesOnLeftOrRight_Names()
        {
            // Arrrange
            recruitmentSystem = new();

            // Act
            Contractor newContractor = new("  Terence", "Tao    ", 100);

            // Assert
            Assert.AreEqual("Terence", newContractor.FirstName);
            Assert.AreEqual("Tao", newContractor.LastName);
        }

        [DataTestMethod]
        [DataRow(0d)]
        [DataRow(-100d)]
        [DataRow(-1d)]
        public void AddContractor_InvalidHourlyWage(double hourlyWage)
        {
            // Arrange
            recruitmentSystem = new();

            // Act and Assert
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddContractor(new Contractor("Terence", "Tao", hourlyWage)));
        }

        [TestMethod]
        public void AddContractor_ExistingFirstLastName()
        {
            // Act
            recruitmentSystem.AddContractor(new Contractor("Cedric", "Anover", 50));

            // Obtain Cedrics
            List<Contractor> duplicateList = recruitmentSystem.Contractors.Where(c => c.FirstName == "Cedric" && c.LastName == "Anover").ToList();

            Contractor cedric1 = duplicateList[0];
            Contractor cedric2 = duplicateList[1];

            Assert.AreEqual(2, duplicateList.Count);
            Assert.AreNotEqual(cedric1.ID, cedric2.ID);
            Assert.AreEqual(cedric1.FirstName, cedric2.FirstName);
            Assert.AreEqual(cedric1.LastName, cedric2.LastName);
            Assert.AreEqual(cedric1.HourlyWage, cedric2.HourlyWage);
        }

        [TestMethod]
        public void AddContractor_InvalidCustomUID()
        {
            // Arrange
            recruitmentSystem = new();
            recruitmentSystem.AddContractor(new Contractor("aaaaa", "Cedric", "Anover", 50));

            // Act and Assert
            Assert.ThrowsException<Exception>(() => recruitmentSystem.AddContractor(new Contractor("aaaaa", "Cedric", "Anover", 50)));
        }

        [TestMethod]
        public void AddContractor_ValidCustomUID()
        {
            // Arrange
            recruitmentSystem = new();
            recruitmentSystem.AddContractor(new Contractor("aaaaa", "Cedric", "Anover", 50));

            // Act
            recruitmentSystem.AddContractor(new Contractor("bbbbb", "Cedric", "Anover", 50));

            // Assert
            Assert.AreEqual(2, recruitmentSystem.Contractors.Count);
        }

        [TestMethod]
        public void RemoveContractor_ContractorAvailable()
        {
            // Arrange
            Contractor contractorToRemove = recruitmentSystem.Contractors.Where(x => x.FirstName == "Terence").ToArray()[0];

            // Act
            recruitmentSystem.RemoveContractor(contractorToRemove);

            // Assert
            Assert.AreEqual(recruitmentSystem.Contractors.Count, 2);
            CollectionAssert.DoesNotContain(recruitmentSystem.Contractors, contractorToRemove);
        }

        [TestMethod]
        public void RemoveContractor_ContractorUnavailable()
        {
            // Arrange
            Contractor contractor = recruitmentSystem.Contractors.Where(x => x.FirstName == "Terence").ToArray()[0];
            Job job = recruitmentSystem.Jobs.Where(x => x.Title == "Maths Professor").ToArray()[0];
            recruitmentSystem.AssignJob(job, contractor);

            // Act
            recruitmentSystem.RemoveContractor(contractor);

            // Assert
            Assert.AreEqual(recruitmentSystem.Contractors.Count, 2);
            Assert.AreEqual(recruitmentSystem.Jobs.Count, 3);
            CollectionAssert.DoesNotContain(recruitmentSystem.Contractors, contractor);
            Assert.IsNull(contractor.StartDate);  // Contractor is still in the memory.
            Assert.IsNull(job.ContractorAssigned);
        }

        [TestMethod]
        public void AddJob_InvalidParameters()
        {
            // Arrange
            recruitmentSystem = new();

            // === Act and Assert ===
            // Invalid Title
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddJob(new Job("", new DateTime(2024, 3, 15), 70000)));
            Assert.ThrowsException<ArgumentException>(() => recruitmentSystem.AddJob(new Job("  ", new DateTime(2024, 3, 15), 70000)));

            // Invalid Date 
            Assert.ThrowsException<Exception>(() => recruitmentSystem.AddJob(new Job("Mathematician", DateTime.Now, 250000)));
            Assert.ThrowsException<Exception>(() => recruitmentSystem.AddJob(new Job("Mathematician", DateTime.Now.AddDays(-1), 250000)));
            Assert.ThrowsException<Exception>(() => recruitmentSystem.AddJob(new Job("Mathematician", DateTime.Now.AddDays(-30), 250000)));

            // Invalid Cost
            Assert.ThrowsException<Exception>(() => recruitmentSystem.AddJob(new Job("Mathematician", new DateTime(2024, 2, 24), 0)));
            Assert.ThrowsException<Exception>(() => recruitmentSystem.AddJob(new Job("Mathematician", new DateTime(2024, 2, 24), -1)));
            Assert.ThrowsException<Exception>(() => recruitmentSystem.AddJob(new Job("Mathematician", new DateTime(2024, 2, 24), -20000)));
        }

        [DataTestMethod]
        [DataRow("   Mathematician  ")]
        [DataRow("    Mathematician")]
        [DataRow("Mathematician    ")]
        public void AddJob_TitleSpaces(string title)
        {
            // Arrange
            recruitmentSystem = new();
            Job newJob = new(title, new DateTime(2024, 1, 18), 250000);

            // Act
            recruitmentSystem.AddJob(newJob);

            // Assert
            Assert.AreEqual(recruitmentSystem.Jobs.Count, 1);
            CollectionAssert.Contains(recruitmentSystem.Jobs, newJob);
            Assert.AreEqual(newJob.Title, "Mathematician");
        }

        [DataTestMethod]
        [DataRow("Mathematician", 1)]
        [DataRow("Mathematician", 1000000)]
        [DataRow("   Mathematician ", 1000000)]
        public void AddJob_ValidParameters(string title, double cost)
        {
            // Arrange
            recruitmentSystem = new();

            DateTime date = DateTime.Now.AddDays(1);
            Job newJob = new(title, date, cost);

            // Act
            recruitmentSystem.AddJob(newJob);

            // Assert
            Assert.AreEqual(recruitmentSystem.Jobs.Count, 1);
            CollectionAssert.Contains(recruitmentSystem.Jobs, newJob);
        }

        [TestMethod]
        public void ViewAvailableContractors_NoContractors()
        {
            // Arrange
            recruitmentSystem = new();
            recruitmentSystem.AddJob(new Job("Mathematician", new DateTime(2024, 2, 24), 250000));
            recruitmentSystem.AddJob(new Job("Maths Professor", new DateTime(2024, 1, 18), 300000));
            recruitmentSystem.AddJob(new Job("Algorithmic Trader", new DateTime(2024, 6, 2), 500000));

            // Act
            List<Contractor> availableContractors = recruitmentSystem.GetAvailableContractors();

            // Assert
            Assert.AreEqual(availableContractors.Count, 0);
        }

        [TestMethod]
        public void ViewAvailableContractors_NoJobs()
        {
            // Arrange
            recruitmentSystem = new();
            recruitmentSystem.AddContractor(new Contractor("Cedric", "Anover", 50));
            recruitmentSystem.AddContractor(new Contractor("David", "Hilbert", 65));
            recruitmentSystem.AddContractor(new Contractor("Terence", "Tao", 100));

            // Act
            List<Contractor> availableContractors = recruitmentSystem.GetAvailableContractors();

            // Assert
            Assert.AreEqual(availableContractors.Count, 3);

            foreach(Contractor contractor in recruitmentSystem.Contractors)
            {
                Assert.IsNull(contractor.StartDate);
                Assert.IsTrue(contractor.IsAvailable);
            }
        }

        [TestMethod]
        public void ViewAvailableContractors_SomeAssigned()
        {
            // Arrange
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[0], recruitmentSystem.Contractors[0]);
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[1], recruitmentSystem.Contractors[1]);

            // Act
            List<Contractor> availableContractors = recruitmentSystem.GetAvailableContractors();

            // Assert
            Assert.AreEqual(availableContractors.Count, 1);

            Assert.IsNotNull(recruitmentSystem.Contractors[0].StartDate);
            Assert.IsNotNull(recruitmentSystem.Contractors[1].StartDate);
            Assert.IsNull(recruitmentSystem.Contractors[2].StartDate);

            Assert.IsNotNull(recruitmentSystem.Jobs[0].ContractorAssigned);
            Assert.IsNotNull(recruitmentSystem.Jobs[1].ContractorAssigned);
            Assert.IsNull(recruitmentSystem.Jobs[2].ContractorAssigned);

            Assert.AreEqual(recruitmentSystem.Jobs[0].ContractorAssigned, recruitmentSystem.Contractors[0]);
            Assert.AreEqual(recruitmentSystem.Jobs[1].ContractorAssigned, recruitmentSystem.Contractors[1]);
        }

        [TestMethod]
        public void ViewAvailableContractors_AllAssigned()
        {
            // Arrange
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[0], recruitmentSystem.Contractors[0]);
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[1], recruitmentSystem.Contractors[1]);
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[2], recruitmentSystem.Contractors[2]);

            // Act
            List<Contractor> availableContractors = recruitmentSystem.GetAvailableContractors();

            // Assert
            Assert.AreEqual(availableContractors.Count, 0);

            foreach(Contractor contractor in recruitmentSystem.Contractors)
            {
                Assert.IsNotNull(contractor.StartDate);
                Assert.IsFalse(contractor.IsAvailable);
            }

            foreach (Job job in recruitmentSystem.Jobs)
            {
                Assert.IsNotNull(job.ContractorAssigned);
            }

            for(int i = 0; i < recruitmentSystem.Contractors.Count; i++)
            {
                Assert.AreEqual(recruitmentSystem.Jobs[i].ContractorAssigned, recruitmentSystem.Contractors[i]);
            }
        }

        [TestMethod]
        public void ViewAvailableContractors_AllNotAssigned()
        {
            // Act
            List<Contractor> availableContractors = recruitmentSystem.GetAvailableContractors();

            // Assert
            Assert.AreEqual(availableContractors.Count, 3);

            foreach (Contractor contractor in recruitmentSystem.Contractors)
            {
                Assert.IsNull(contractor.StartDate);
                Assert.IsTrue(contractor.IsAvailable);
            }

            foreach (Job job in recruitmentSystem.Jobs)
            {
                Assert.IsNull(job.ContractorAssigned);
            }
        }

        [TestMethod]
        public void ViewUnassignedJobs_NoJobs()
        {
            // Arrange
            recruitmentSystem = new();
            recruitmentSystem.AddContractor(new Contractor("Cedric", "Anover", 50));
            recruitmentSystem.AddContractor(new Contractor("David", "Hilbert", 65));
            recruitmentSystem.AddContractor(new Contractor("Terence", "Tao", 100));

            // Act
            List<Job> unassignedJobs = recruitmentSystem.GetUnassignedJobs();

            // Assert
            Assert.AreEqual(unassignedJobs.Count, 0);

            foreach(Contractor contractor in recruitmentSystem.Contractors)
            {
                Assert.IsNull(contractor.StartDate); 
                Assert.IsTrue(contractor.IsAvailable);
            }
        }

        [TestMethod]
        public void ViewUnassignedJobs_NoContractors()
        {
            // Arrange
            recruitmentSystem = new();
            recruitmentSystem.AddJob(new Job("Mathematician", new DateTime(2024, 2, 24), 250000));
            recruitmentSystem.AddJob(new Job("Maths Professor", new DateTime(2024, 1, 18), 300000));
            recruitmentSystem.AddJob(new Job("Algorithmic Trader", new DateTime(2024, 6, 2), 500000));

            // Act
            List<Job> unassignedJobs = recruitmentSystem.GetUnassignedJobs();

            // Assert
            Assert.AreEqual(unassignedJobs.Count, 3);

            foreach(Job job in recruitmentSystem.Jobs)
            {
                Assert.IsNull(job.ContractorAssigned);
            }
        }

        [TestMethod]
        public void ViewUnassignedJobs_SomeAssigned()
        {
            // Note: This scenario is similar to ViewAvailableContractors_SomeAssigned

            // Arrange
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[0], recruitmentSystem.Contractors[0]);
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[1], recruitmentSystem.Contractors[1]);

            // Act
            List<Job> unassignedJobs = recruitmentSystem.GetUnassignedJobs();

            // Assert
            Assert.AreEqual(unassignedJobs.Count, 1);

            Assert.IsNotNull(recruitmentSystem.Contractors[0].StartDate);
            Assert.IsNotNull(recruitmentSystem.Contractors[1].StartDate);
            Assert.IsNull(recruitmentSystem.Contractors[2].StartDate);  // The Unassigned Job

            Assert.IsNotNull(recruitmentSystem.Jobs[0].ContractorAssigned);
            Assert.IsNotNull(recruitmentSystem.Jobs[1].ContractorAssigned);
            Assert.IsNull(recruitmentSystem.Jobs[2].ContractorAssigned);

            Assert.AreEqual(recruitmentSystem.Jobs[0].ContractorAssigned, recruitmentSystem.Contractors[0]);
            Assert.AreEqual(recruitmentSystem.Jobs[1].ContractorAssigned, recruitmentSystem.Contractors[1]);
        }

        [TestMethod]
        public void ViewUnassignedJobs_AllAssigned()
        {
            // Note: This scenario is similar to ViewAvailableContractors_AllAssigned

            // Arrange
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[0], recruitmentSystem.Contractors[0]);
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[1], recruitmentSystem.Contractors[1]);
            recruitmentSystem.AssignJob(recruitmentSystem.Jobs[2], recruitmentSystem.Contractors[2]);

            // Act
            List<Job> unassignedJobs = recruitmentSystem.GetUnassignedJobs();

            // Assert
            Assert.AreEqual(unassignedJobs.Count, 0);

            foreach (Contractor contractor in recruitmentSystem.Contractors)
            {
                Assert.IsNotNull(contractor.StartDate);
                Assert.IsFalse(contractor.IsAvailable);
            }

            foreach (Job job in recruitmentSystem.Jobs)
            {
                Assert.IsNotNull(job.ContractorAssigned);
            }

            for (int i = 0; i < recruitmentSystem.Contractors.Count; i++)
            {
                Assert.AreEqual(recruitmentSystem.Jobs[i].ContractorAssigned, recruitmentSystem.Contractors[i]);
            }
        }

        [TestMethod]
        public void ViewUnassignedJobs_AllUnassigned()
        {
            // Note: This scenario is similar to ViewAvailableContractors_AllNotAssigned

            // Act
            List<Job> unassignedJobs = recruitmentSystem.GetUnassignedJobs();

            // Assert
            Assert.AreEqual(unassignedJobs.Count, 3);

            foreach (Contractor contractor in recruitmentSystem.Contractors)
            {
                Assert.IsNull(contractor.StartDate);
                Assert.IsTrue(contractor.IsAvailable);
            }

            foreach (Job job in recruitmentSystem.Jobs)
            {
                Assert.IsNull(job.ContractorAssigned);
            }
        }
    }
}