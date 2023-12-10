namespace FuzzyMath;

[Serializable]
public class NotDefinedException : Exception
{
    public NotDefinedException(){ }

    public NotDefinedException(string message): base(message){ }

    public NotDefinedException(string message, Exception innerException): base(message, innerException){ }
}