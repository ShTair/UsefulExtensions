﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShComp
{
    /// <summary>
    /// セマフォをうまく使う拡張メソッドです。
    /// </summary>
    static class SemaphoreExtensions
    {
        /// <summary>
        /// <see cref="System.Threading.SemaphoreSlim"/> を用いた、非同期処理中の排他処理を実現します。
        /// </summary>
        /// <param name="func">排他で行いたい処理</param>
        public static async Task Critical(this SemaphoreSlim s, Func<Task> func)
        {
            try { await s.WaitAsync(); await func(); }
            finally { s.Release(); }
        }

        /// <summary>
        /// <see cref="System.Threading.SemaphoreSlim"/> を用いた、値を返したい非同期処理中の排他処理を実現します。
        /// </summary>
        /// <param name="func">排他で行いたい処理</param>
        /// <typeparam name="T">返す値の型</typeparam>
        public static async Task<T> Critical<T>(this SemaphoreSlim s, Func<Task<T>> func)
        {
            try { await s.WaitAsync(); return await func(); }
            finally { s.Release(); }
        }

        /// <summary>
        /// <see cref="System.Threading.SemaphoreSlim"/> を用いた、非同期処理中の排他処理を実現します。
        /// </summary>
        /// <param name="func">排他で行いたい処理</param>
        public static async Task Critical(this SemaphoreSlim s, Action func)
        {
            try { await s.WaitAsync(); func(); }
            finally { s.Release(); }
        }

        /// <summary>
        /// <see cref="System.Threading.SemaphoreSlim"/> を用いた、値を返したい非同期処理中の排他処理を実現します。
        /// </summary>
        /// <param name="func">排他で行いたい処理</param>
        /// <typeparam name="T">返す値の型</typeparam>
        public static async Task<T> Critical<T>(this SemaphoreSlim s, Func<T> func)
        {
            try { await s.WaitAsync(); return func(); }
            finally { s.Release(); }
        }
    }
}
