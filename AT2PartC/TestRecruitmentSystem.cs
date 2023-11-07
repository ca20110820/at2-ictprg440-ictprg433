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
    }
}