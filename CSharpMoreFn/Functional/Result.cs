using System;

namespace CSharpMoreFn.Functional
{
    public readonly struct Result<T, TError>
    {
        private readonly bool _isSuccess;
        private readonly T _value;
        private readonly TError _error;

        private Result(T value)
        {
            _isSuccess = true;
            _value = value;
            _error = default!;
        }

        private Result(TError error, bool isFailure)
        {
            _isSuccess = !isFailure;
            _value = default!;
            _error = error;
        }

        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !_isSuccess;

        public static Result<T, TError> Ok(T value) => new(value);
        public static Result<T, TError> Fail(TError error) => new(error, true);

        public Result<TResult, TError> Map<TResult>(Func<T, TResult> map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            return _isSuccess ? Result<TResult, TError>.Ok(map(_value)) : Result<TResult, TError>.Fail(_error);
        }

        public Result<TResult, TError> Bind<TResult>(Func<T, Result<TResult, TError>> bind)
        {
            if (bind == null) throw new ArgumentNullException(nameof(bind));
            return _isSuccess ? bind(_value) : Result<TResult, TError>.Fail(_error);
        }

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<TError, TResult> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));
            return _isSuccess ? onSuccess(_value) : onFailure(_error);
        }

        public Result<T, TError> Tap(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (_isSuccess) action(_value);
            return this;
        }

        public Result<T, TError> TapError(Action<TError> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (!_isSuccess) action(_error);
            return this;
        }

        public T ValueOr(Func<TError, T> fallback)
        {
            if (fallback == null) throw new ArgumentNullException(nameof(fallback));
            return _isSuccess ? _value : fallback(_error);
        }

        public override string ToString() => _isSuccess ? $"Ok({_value})" : $"Fail({_error})";
    }
}
