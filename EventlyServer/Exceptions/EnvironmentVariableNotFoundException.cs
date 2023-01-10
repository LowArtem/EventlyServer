namespace EventlyServer.Exceptions;

/// <summary>
/// Исключение обозначает, что не была передана необходимая переменная окружения
/// </summary>
public class EnvironmentVariableNotFoundException : Exception
{
    /// <summary>
    /// Создание исключения
    /// </summary>
    public EnvironmentVariableNotFoundException()
        : base("Environment variable not set")
    {
    }
    
    /// <summary>
    /// Создание исключения
    /// </summary>
    /// <param name="variableName">название отсутствующей переменной</param>
    public EnvironmentVariableNotFoundException(string variableName)
        : base($"Environment variable <{variableName}> is not set")
    {
    }
    
    /// <summary>
    /// Создание исключения
    /// </summary>
    /// <param name="variablesNames">названия отсутствующих переменных</param>
    public EnvironmentVariableNotFoundException(params string[] variablesNames)
        : base($"Environment variables (one or more): <{string.Join(", ", variablesNames)}> is not set")
    {
    }
}