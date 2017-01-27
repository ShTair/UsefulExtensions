using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShComp
{
    /// <summary>
    /// 配列やListなど、列挙体に対する拡張メソッドです。
    /// </summary>
    static class EnumerableExtensions
    {
        /// <summary>
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> の各要素に対して、指定された処理を実行します。<para />
        /// これを実行すると、列挙が実行されます。列挙済みの <see cref="System.Collections.Generic.IEnumerable{T}"/> に対して実行することをお勧めします。<para />
        /// </summary>
        /// <param name="action">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> の各要素に対して実行する <see cref="System.Action{T}"> デリゲート。
        /// </param>
        public static void ForEach<T>(this IEnumerable<T> src, Action<T> action)
        {
            foreach (var item in src)
            {
                action(item);
            }
        }

        /// <summary>
        /// 前処理と後処理を行う ForEach です。<para /> 
        /// この <see cref="System.Collections.Generic.IEnumerable{T}"/> に要素が存在する場合のみ、前処理と後処理が実行されます。
        /// </summary>
        /// <param name="action">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> の各要素に対して実行する <see cref="System.Action{T}"> デリゲート。
        /// </param>
        /// <param name="firstAction">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> に要素が存在する場合に、各要素に対するactionの前に実行される処理。
        /// <see cref="null"/> を指定した場合、何もしません。
        /// </param>
        /// <param name="finalAction">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> に要素が存在する場合に、各要素に対するactionの後に実行される処理。<para />
        /// 前処理や各要素に対するactionで例外が発生した場合でも実行されます。<para />
        /// <see cref="null"/> を指定した場合、何もしません。
        /// </param>
        public static void ForEach<T>(this IEnumerable<T> src, Action<T> action, Action firstAction, Action finalAction)
        {
            var e = src.GetEnumerator();
            if (!e.MoveNext()) return;

            try
            {
                firstAction?.Invoke();
                do action(e.Current); while (e.MoveNext());
            }
            finally
            {
                finalAction?.Invoke();
            }
        }

        /// <summary>
        /// 前処理と後処理を行う ForEach です。<para /> 
        /// この <see cref="System.Collections.Generic.IEnumerable{T}"/> に要素が存在する場合のみ、前処理と後処理が実行されます。<para />
        /// 非同期な処理用です。
        /// </summary>
        /// <param name="action">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> の各要素に対して実行する <see cref="System.Func{T, TResult}"> デリゲート。
        /// 順に実行されます。
        /// </param>
        /// <param name="firstAction">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> に要素が存在する場合に、各要素に対するactionの前に実行される処理。
        /// <see cref="null"/> を指定した場合、何もしません。
        /// </param>
        /// <param name="finalAction">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> に要素が存在する場合に、各要素に対するactionの後に実行される処理。<para />
        /// 前処理や各要素に対するactionで例外が発生した場合でも実行されます。<para />
        /// <see cref="null"/> を指定した場合、何もしません。
        /// </param>
        public static async Task ForEach<T>(this IEnumerable<T> src, Func<T, Task> func, Func<Task> firstFunc, Func<Task> finalFunc)
        {
            var e = src.GetEnumerator();
            if (!e.MoveNext()) return;

            try
            {
                await firstFunc?.Invoke();
                do await func(e.Current); while (e.MoveNext());
            }
            finally
            {
                await finalFunc?.Invoke();
            }
        }

        /// <summary>
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> の各要素に対して、指定された処理を同期的に実行します。<para />
        /// 列挙に対して、順番に非同期処理を実行したい場合に使います。<para /> 
        /// </summary>
        /// <param name="action">
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/> の各要素に対して実行する <see cref="System.Func{T, TResult}"> デリゲート。
        /// </param>
        public static async Task ForEach<T>(this IEnumerable<T> src, Func<T, Task> func)
        {
            foreach (var item in src)
            {
                await func(item);
            }
        }
    }
}
