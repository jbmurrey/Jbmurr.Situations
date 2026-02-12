namespace Situations.Sample
{
	public interface IPositionRepository
	{
		bool IsManager(int positionId);
		int GetManagerOf(int employeeId);
	}
}
