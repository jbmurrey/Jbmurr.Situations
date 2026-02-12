namespace Situations.Sample
{
    public class EmployeeService : IEmployeeService
    {
        public void AddEmployee(Employee employee) { }

        public bool EmployeeExist(int id)
        {
            return true;
        }
    }
}
