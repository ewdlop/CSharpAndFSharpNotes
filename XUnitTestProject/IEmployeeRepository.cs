using System;
using System.Numerics;

namespace XUnitTestProject;



public class Calculator<T> where T: INumber<T>
{
    public T Add(T a, T b)
    {
        return a + b;
    }
}
public class Computer
{
    public virtual int Add(int a, int b)
    {
        return a + b;
    }

    protected virtual int Multiply(int a, int b)
    {
        return a * b;
    }
}
public interface IEmployeeRepository : ICountable
{
    bool TryGetEmployee(int id, out Employee employee);
    Employee BestEmployee { get; set; }
    event EventHandler BestEmployeeChangd;
}
public interface ICountable
{
    int Count { get; }
}