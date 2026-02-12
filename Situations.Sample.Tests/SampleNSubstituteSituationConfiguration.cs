using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Situations.Core;
using Situations.Moq;


namespace Situations.Sample.Tests
{
    public static class SampleNSubstituteSituationConfiguration
    {
        public static SituationsContainer<EmployeeCreationSituations> GetSituationsContainer()
        {
            var builder = new NSubstituteSituationsBuilder<EmployeeCreationSituations>();
            builder.RegisterService<EmployeeCreationService>();
            builder.RegisterService(_ => Substitute.For<ILoggingService>());

            builder
                .RegisterSituation<IPositionRepository>(EmployeeCreationSituations.RequestorIsManager)
                .OnInvocation(x => x.IsManager(TestingConstants.ManagerId).Returns(true));
            builder
                .RegisterSituation<IPositionRepository>(EmployeeCreationSituations.RequesterIsNotManager)
                .OnInvocation(x => x.IsManager(TestingConstants.EmployeeId).Returns(false));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeTryingToAddedDoesNotExist)
                .OnInvocation(x => x.EmployeeExist(TestingConstants.EmployeeId).Returns(false));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeTryingToBeAddedExist)
                .OnInvocation(x => x.EmployeeExist(TestingConstants.EmployeeId).Returns(true));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeWasAdded)
                .OnInvocation(x => x.Received().AddEmployee(TestingConstants.Employee));
            builder
                .RegisterSituation<IEmployeeService>(EmployeeCreationSituations.EmployeeWasNotAdded)
                .OnInvocation(x => x.DidNotReceive().AddEmployee(TestingConstants.Employee));
            builder
                .RegisterSituation<INotificationService>(EmployeeCreationSituations.ManagerWasNotified)
                .OnInvocation(x => x.Received().Notify(TestingConstants.ManagerId, Arg.Any<string>()));
            builder
                .RegisterSituation<INotificationService>(EmployeeCreationSituations.ManagerWasNotNotified)
                .OnInvocation(x => x.DidNotReceive().Notify(TestingConstants.ManagerId, Arg.Any<string>()));
            builder
                .RegisterSituation<IPositionRepository>(EmployeeCreationSituations.ManagerOfEmployeeIsFound)
                .OnInvocation(x => x.GetManagerOf(TestingConstants.EmployeeId).Returns(TestingConstants.ManagerId));

            return builder.Build();
        }
    }
}
