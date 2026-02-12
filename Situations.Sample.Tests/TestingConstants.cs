namespace Situations.Sample.Tests
{
    public static class TestingConstants
	{
		public static int EmployeeId = 100;
		public static int ManagerId = 200;
		public static string NotificationMessage = "NotificationMessage";
		public static Employee Employee = new Employee
		{
			FullName = "Bob",
			Id = EmployeeId,
			JobCode = 12345
		};
	}
}
