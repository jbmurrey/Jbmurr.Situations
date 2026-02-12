namespace Situations.Sample.Tests
{
    public enum EmployeeCreationSituations
    {
        RequestorIsManager,
        RequesterIsNotManager,
        EmployeeTryingToBeAddedExist,
        EmployeeTryingToAddedDoesNotExist,
        EmployeeWasAdded,
        EmployeeWasNotAdded,
        ManagerWasNotified,
        ManagerWasNotNotified,
        ManagerOfEmployeeIsFound
    }
}
