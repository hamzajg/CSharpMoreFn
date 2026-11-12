using System;

namespace CSharpMoreFn.Functional
{
    public static class FunctionalExtensions
    {
        public static TResult Pipe<TSource, TResult>(this TSource source, Func<TSource, TResult> transform)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            return transform(source);
        }

        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T2, T3> g, Func<T1, T2> f)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));
            if (f == null) throw new ArgumentNullException(nameof(f));
            return x => g(f(x));
        }

        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return a => b => func(a, b);
        }

        public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return a => b => c => func(a, b, c);
        }
    }
}
