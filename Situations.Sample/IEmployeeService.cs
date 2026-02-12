namespace Situations.Sample
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        bool EmployeeExist(int id);
    }
}
