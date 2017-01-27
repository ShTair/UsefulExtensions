using System;
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
        /// <see cref="System.Threading.SemaphoreSlim"/> を用いた、非同期処理中の排他処理を実現します。<para />
        /// 使い方は、このコードのコメントを見てください。
        /// </summary>
        public static async Task<IDisposable> Lock(this SemaphoreSlim s)
        {
            await s.WaitAsync();
            return new _LockObj(s);

            // 使い方
            // まず、どこかで SemaphoreSlim を作っておきます。
            // 普通の lock 文のように1スレッドだけしか認めない場合は initialCount は 1 で。

            // var s = new SemaphoreSlim(1);

            // 排他処理したい処理を、以下のように書きます。
            // lock ではなく using です。間違えないように。

            // using (await s.Lock())
            // {
            //    // 排他処理したい処理
            // }
        }

        private class _LockObj : IDisposable
        {
            private SemaphoreSlim _s;

            public _LockObj(SemaphoreSlim s)
            {
                _s = s;
            }

            public void Dispose()
            {
                var s = _s;
                if (s == null) return;
                lock (s)
                {
                    if (_s == null) return;
                    _s = null;

                    s.Release();
                }
            }
        }
    }
}
