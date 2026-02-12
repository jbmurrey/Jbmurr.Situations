using Moq;
using Situations.Core;
using Situations.Moq;

namespace Situations.Sample.Tests
{
    public static class SampleMoqSituationConfiguration
    {
        public static SituationsContainer<EmployeeCreationSituations> GetSituationsContainer()
        {
            var builder = new MoqSituationsBuilder<EmployeeCreationSituations>();

            builder.RegisterService<EmployeeCreationService>();
            builder.AddMock<ILoggingService>();

            builder
                .RegisterSituation<IPositionRepository>(EmployeeCreationSituations.RequestorIsManager)
                .OnInvocation(mock => mock.Setup((x) => x.IsManager(TestingConstants.ManagerId)).Returns(true));
            builder
                .RegisterSituation<IPositionRepository>(EmployeeCreationSituations.RequesterIsNotManager)
                .OnInvocation(mock => mock.Setup((x) => x.IsManager(TestingConstants.EmployeeId)).Returns(false));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeTryingToAddedDoesNotExist)
                .OnInvocation(mock => mock.Setup((x) => x.EmployeeExist(TestingConstants.EmployeeId)).Returns(false));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeTryingToBeAddedExist)
                .OnInvocation(mock => mock.Setup((x) => x.EmployeeExist(TestingConstants.EmployeeId)).Returns(true));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeWasAdded)
                .OnInvocation(mock => mock.Verify(x => x.AddEmployee(TestingConstants.Employee), Times.Once));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeWasNotAdded)
                .OnInvocation(mock => mock.Verify(x => x.AddEmployee(TestingConstants.Employee), Times.Never));
            builder
                .RegisterSituation<INotificationService>(EmployeeCreationSituations.ManagerWasNotified)
                .OnInvocation(mock => mock.Verify(x => x.Notify(TestingConstants.ManagerId, It.IsAny<string>()), Times.Once));
            builder
                .RegisterSituation<INotificationService>(EmployeeCreationSituations.ManagerWasNotNotified)
                .OnInvocation(mock => mock.Verify(x => x.Notify(TestingConstants.ManagerId, It.IsAny<string>()), Times.Never));
            builder
                .RegisterSituation<IPositionRepository>(EmployeeCreationSituations.ManagerOfEmployeeIsFound)
                .OnInvocation(mock => mock.Setup(x => x.GetManagerOf(TestingConstants.EmployeeId)).Returns(TestingConstants.ManagerId));

            return builder.Build();
        }
    }
}
