namespace Situations.Sample
{
    public class EmployeeCreationService(IEmployeeService employeeService, IPositionRepository positionRepository, INotificationService notificationService, ILoggingService loggingService)
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly IPositionRepository _positionRepository = positionRepository;
        private readonly INotificationService _notificationService = notificationService;
        private readonly ILoggingService _loggingService = loggingService;

        public void AddEmployee(int requestUserId, Employee employee)
        {
            if (_positionRepository.IsManager(requestUserId))
            {
                if (!_employeeService.EmployeeExist(employee.Id))
                {
                    _loggingService.Log($"Adding employee {employee.Id}");
                    _employeeService.AddEmployee(employee);
                }
            }
            else
            {
                var managerId = _positionRepository.GetManagerOf(requestUserId);
                _notificationService.Notify(managerId, $"{requestUserId} is attempting to hire an Employee.");
            }
        }
    }
}
