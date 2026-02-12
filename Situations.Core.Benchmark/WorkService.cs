namespace Situations.Core.Benchmark
{
    public class DependencyA
    {
        public void DoA()
        {
        }
    }

    public class DependencyB
    {
        public void DoB()
        {
        }
    }

    public class DependencyC
    {
        public void DoC()
        {
        }
    }

    public class WorkService
    {
        private readonly DependencyA _depA;
        private readonly DependencyB _depB;
        private readonly DependencyC _depC;

        public WorkService(DependencyA depA, DependencyB depB, DependencyC depC)
        {
            _depA = depA;
            _depB = depB;
            _depC = depC;
        }

        public void Work()
        {
            _depA.DoA();
            _depB.DoB();
            _depC.DoC();
        }
    }
}
