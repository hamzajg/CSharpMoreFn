using System;
using System.Collections.Generic;

namespace CSharpMoreFn.Functional
{
    public readonly struct Option<T> : IEquatable<Option<T>>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        private Option(T value)
        {
            _hasValue = value is not null;
            _value = value;
        }

        public bool IsSome => _hasValue;
        public bool IsNone => !_hasValue;

        public static Option<T> Some(T value) => new(value ?? throw new ArgumentNullException(nameof(value)));
        public static Option<T> None => default;

        public Option<TResult> Map<TResult>(Func<T, TResult> map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            return _hasValue ? Option<TResult>.Some(map(_value)) : Option<TResult>.None;
        }

        public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> bind)
        {
            if (bind == null) throw new ArgumentNullException(nameof(bind));
            return _hasValue ? bind(_value) : Option<TResult>.None;
        }

        public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
        {
            if (onSome == null) throw new ArgumentNullException(nameof(onSome));
            if (onNone == null) throw new ArgumentNullException(nameof(onNone));
            return _hasValue ? onSome(_value) : onNone();
        }

        public void Match(Action<T> onSome, Action onNone)
        {
            if (onSome == null) throw new ArgumentNullException(nameof(onSome));
            if (onNone == null) throw new ArgumentNullException(nameof(onNone));
            if (_hasValue) onSome(_value);
            else onNone();
        }

        public Option<T> Where(Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return _hasValue && predicate(_value) ? this : None;
        }

        public T GetOrElse(T fallback) => _hasValue ? _value : fallback;

        public T GetOrElse(Func<T> fallback)
        {
            if (fallback == null) throw new ArgumentNullException(nameof(fallback));
            return _hasValue ? _value : fallback();
        }

        public bool Equals(Option<T> other)
        {
            if (!_hasValue && !other._hasValue) return true;
            if (_hasValue != other._hasValue) return false;
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

        public override int GetHashCode() => _hasValue && _value is not null ? _value.GetHashCode() : 0;

        public override string ToString() => _hasValue ? $"Some({_value})" : "None";

        public static implicit operator Option<T>(T? value) => value is not null ? Some(value) : None;
    }
}
