namespace SmartServiceBooking.Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.None);

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
        => IsSuccess ? onSuccess() : onFailure(Error);

    public Result Tap(Action action)
    {
        if (IsSuccess) action();
        return this;
    }

    public async Task<Result> TapAsync(Func<Task> func)
    {
        if (IsSuccess) await func();
        return this;
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<Error, TResult> onFailure)
        => IsSuccess ? onSuccess(Value) : onFailure(Error);

    public Result<TResult> Map<TResult>(Func<TValue, TResult> func)
        => IsSuccess ? Result.Success(func(Value)) : Result.Failure<TResult>(Error);

    public Result<TResult> Bind<TResult>(Func<TValue, Result<TResult>> func)
        => IsSuccess ? func(Value) : Result.Failure<TResult>(Error);

    public async Task<Result<TResult>> BindAsync<TResult>(Func<TValue, Task<Result<TResult>>> func)
        => IsSuccess ? await func(Value) : Result.Failure<TResult>(Error);

    public Result<TValue> Tap(Action<TValue> action)
    {
        if (IsSuccess) action(Value);
        return this;
    }

    public async Task<Result<TValue>> TapAsync(Func<TValue, Task> func)
    {
        if (IsSuccess) await func(Value);
        return this;
    }
}
