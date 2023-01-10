namespace EventlyServer.Extensions;

/// <summary>
/// Результат любой операции (вместо выбрасывания исключения)
/// </summary>
/// <typeparam name="T">Тип успешного результата</typeparam>
public struct Result<T>
{
    /// <summary>Значение, если успешно</summary>
    public T? Value { get; }
    
    /// <summary>Исключение, если провал</summary>
    public Exception? Exception { get; }

    /// <summary>Успешна ли операция</summary>
    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Value))]
    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Создать успешный объект <c>Result</c>
    /// </summary>
    /// <param name="value">Успешно полученное значение</param>
    public Result(T value)
    {
        IsSuccess = true;
        Exception = null;
        Value = value;
    }

    /// <summary>
    /// Создать провальный объект <c>Result</c>
    /// </summary>
    /// <param name="exception">Исключение, описывающее причину провала</param>
    /// <exception cref="ArgumentNullException">Если исключение равно null</exception>
    public Result(Exception exception)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        IsSuccess = false;
        Value = default;
    }

    public static implicit operator Result<T>(T value)
        => new(value);

    public static implicit operator Result<T>(Exception exception)
        => new(exception);
    
    /// <summary>
    /// Сравнение типа исключения
    /// </summary>
    /// <typeparam name="TException">Тип исключения для сравнения с текущим</typeparam>
    /// <returns>true - если тот же тип, иначе false</returns>
    public bool ExceptionIs<TException>()
        where TException : Exception
        => Exception is TException;

    /// <summary>
    /// Конвертация в результат другого типа
    /// </summary>
    /// <param name="converter">Функция конвертации</param>
    /// <typeparam name="TEnd">Другой тип результата</typeparam>
    /// <returns>Результат типа <typeparamref name="TEnd"/></returns>
    public Result<TEnd> ConvertToAnotherResult<TEnd>(Func<T, TEnd> converter)
        => IsSuccess
            ? new Result<TEnd>(converter(Value))
            : new Result<TEnd>(Exception);

    /// <summary>
    /// Конвертация в пустой результат с потерей значения
    /// </summary>
    /// <returns>результат, зависящий от успешности</returns>
    public Result ConvertToEmptyResult()
        => IsSuccess
            ? new Result(true)
            : new Result(Exception);

    public void Deconstruct(out bool success, out T? value)
        => (success, value) = (IsSuccess, Value);

    public void Deconstruct(out bool success, out T? value, out Exception? exception)
        => (success, value, exception) = (IsSuccess, Value, Exception);

    public static implicit operator bool(Result<T> result)
        => result.IsSuccess;

    public static implicit operator Task<Result<T>>(Result<T> result)
        => result.AsTask;

    /// <summary>Асинхронное представление результата</summary>
    public Task<Result<T>> AsTask => Task.FromResult(this);
}

/// <summary>
/// Результат любой операции (вместо выбрасывания исключения)
/// </summary>
public struct Result
{
    /// <summary>Исключение, если провал</summary>
    public Exception? Exception { get; }
    
    /// <summary>Успешна ли операция</summary>
    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Создать объект <c>Result</c>
    /// </summary>
    /// <param name="success">Успешная ли операция</param>
    public Result(bool success)
    {
        IsSuccess = success;
        Exception = null;
    }

    /// <summary>
    /// Создать провальный объект <c>Result</c>
    /// </summary>
    /// <param name="exception">Исключение, описывающее причину провала</param>
    /// <exception cref="ArgumentNullException">Если исключение равно null</exception>
    public Result(Exception exception)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        IsSuccess = false;
    }

    /// <summary>
    /// Сравнение типа исключения
    /// </summary>
    /// <typeparam name="TException">Тип исключения для сравнения с текущим</typeparam>
    /// <returns>true - если тот же тип, иначе false</returns>
    public bool ExceptionIs<TException>()
        where TException : Exception
        => Exception is TException;

    /// <summary>
    /// Создание успешнного результата
    /// </summary>
    /// <returns>Успешный результат</returns>
    public static Result Success()
        => new(true);
    
    /// <summary>
    /// Создание успешнного результата
    /// </summary>
    /// <param name="value">Результат операции</param>
    /// <typeparam name="T">Тип данных, полученного значения</typeparam>
    /// <returns>Успешный результат</returns>
    public static Result<T> Success<T>(T value)
        => new(value);

    /// <summary>
    /// Создание провального результата
    /// </summary>
    /// <param name="exception">Исключение, описывающее причину провала</param>
    /// <returns>Провальный результат</returns>
    public static Result Fail(Exception exception)
        => new(exception);

    /// <summary>
    /// Создание провального результата
    /// </summary>
    /// <param name="exception">Исключение, описывающее причину провала</param>
    /// <typeparam name="T">Тип данных, который должен был получиться</typeparam>
    /// <returns>Провальный результат</returns>
    public static Result<T> Fail<T>(Exception exception)
        => new(exception);

    public static implicit operator Result(Exception exception)
        => new(exception);

    public static implicit operator bool(Result result)
        => result.IsSuccess;

    public static implicit operator Task<Result>(Result result)
        => result.AsTask;

    public void Deconstruct(out bool success, out Exception? exception)
        => (success, exception) = (IsSuccess, Exception);

    /// <summary>Асинхронное представление результата</summary>
    public Task<Result> AsTask => Task.FromResult(this);
}