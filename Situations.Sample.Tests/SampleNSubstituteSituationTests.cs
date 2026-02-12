using Situations.Core;

namespace Situations.Sample.Tests
{
    [TestClass]
    public class SampleNSubstituteSituationTest
    {
        private readonly SituationsContainer<EmployeeCreationSituations> _configuredServiceProvider = SampleNSubstituteSituationConfiguration.GetSituationsContainer();

        [TestMethod]
        public void AddEmployee_GivenRequestIsManagerAndEmployeeTryingToBeAddedDoesNotExist_EmployeeWasAdded()
        {
            //Arrange
            using var service = _configuredServiceProvider.GetConfiguredService<EmployeeCreationService>();

            service.InvokeSituation(EmployeeCreationSituations.RequestorIsManager);
            service.InvokeSituation(EmployeeCreationSituations.EmployeeTryingToAddedDoesNotExist);

            //Act
            service.Instance.AddEmployee(TestingConstants.ManagerId, TestingConstants.Employee);

            //Assert
            service.InvokeSituation(EmployeeCreationSituations.EmployeeWasAdded);
        }

        [TestMethod]
        public void AddEmployee_GivenRequestIsManagerAndEmployeeTryingToBeAddedDoesExist_EmployeeWasAdded()
        {
            //Arrange
            using var service = _configuredServiceProvider.GetConfiguredService<EmployeeCreationService>();

            service.InvokeSituation(EmployeeCreationSituations.RequestorIsManager);
            service.InvokeSituation(EmployeeCreationSituations.EmployeeTryingToBeAddedExist);

            //Act
            service.Instance.AddEmployee(TestingConstants.ManagerId, TestingConstants.Employee);

            //Assert
            service.InvokeSituation(EmployeeCreationSituations.EmployeeWasNotAdded);
        }

        [TestMethod]
        public void AddEmployee_GivenRequestIsNotManager_NotifyManager()
        {
            //Arrange
            using var service = _configuredServiceProvider.GetConfiguredService<EmployeeCreationService>();
            service.InvokeSituation(EmployeeCreationSituations.RequesterIsNotManager);
            service.InvokeSituation(EmployeeCreationSituations.ManagerOfEmployeeIsFound);

            //Act
            service.Instance.AddEmployee(TestingConstants.EmployeeId, TestingConstants.Employee);

            //Assert
            service.InvokeSituation(EmployeeCreationSituations.ManagerWasNotified);
        }

        [TestMethod]
        public void AddEmployee_GivenRequestIsManager_MangerWasNotNotified()
        {
            //Arrange
            using var service = _configuredServiceProvider.GetConfiguredService<EmployeeCreationService>();
            service.InvokeSituation(EmployeeCreationSituations.RequestorIsManager);
            service.InvokeSituation(EmployeeCreationSituations.ManagerOfEmployeeIsFound);
            //Act
            service.Instance.AddEmployee(TestingConstants.ManagerId, TestingConstants.Employee);

            //Assert
            service.InvokeSituation(EmployeeCreationSituations.ManagerWasNotNotified);
        }
    }
}