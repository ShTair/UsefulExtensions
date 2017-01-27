using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShComp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("拡張メソッドを使いこなしましょう！");
            // このプロジェクトを自分のプロジェクトに追加するより、
            // ほしいファイルのみを自分のプロジェクトにリンクするなり、追加するなりするのがいいと思います。

            EnumerableExtensions_ForEach_前後処理の実験();
            //EnumerableExtensions_ForEach_Asyncの実験().Wait();
            //SemaphoreExtensionsの実験().Wait();
            Console.ReadLine();
        }

        #region EnumerableExtensions

        private static void EnumerableExtensions_ForEach_前後処理の実験()
        {
            Console.WriteLine();
            Console.WriteLine("########## ########## ##########");
            Console.WriteLine("EnumerableExtensions_ForEach_前後処理の実験");

            Console.WriteLine();
            Console.WriteLine("例えばテーブルヘッダを付けたりとか、データベースの保存処理に使います。");

            {
                Console.WriteLine();
                Console.WriteLine("■ 要素がある場合");

                var array = Enumerable.Range(0, 3).Select(t => new { Id = t, Value = t * t });

                array.ForEach(
                    t => Console.WriteLine($"<tr><td>{t.Id}</td><td>{t.Value}</td></tr>"),
                    () => Console.WriteLine("<table><tr><th>ID</th><th>Value</th></tr>"),
                    () => Console.WriteLine("</table>"));

                Console.WriteLine("前後処理が、要素に対する処理の前後に実行されます。");
            }

            {
                Console.WriteLine();
                Console.WriteLine("■ 要素がない場合");

                var array = Enumerable.Range(0, 0).Select(t => new { Id = t, Value = t * t });

                array.ForEach(
                    t => Console.WriteLine($"<tr><td>{t.Id}</td><td>{t.Value}</td></tr>"),
                    () => Console.WriteLine("<table><tr><th>ID</th><th>Value</th></tr>"),
                    () => Console.WriteLine("</table>"));

                Console.WriteLine("前後処理も実行されません。");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("他にも、データベース関連で、書き込むべき処理のリストがあり、");
            Console.WriteLine("全部書き込んだ後に SaveChanges したい場合とか");
            Console.WriteLine("（処理がなかった場合は SaveChanges もしたくない）、");
            Console.WriteLine("そもそも処理が存在する場合だけコンテキストをnewしたい場合とかに使えます。");
            Console.WriteLine("非同期版もあるよ。");
        }

        private static async Task EnumerableExtensions_ForEach_Asyncの実験()
        {
            Console.WriteLine();
            Console.WriteLine("########## ########## ##########");
            Console.WriteLine("EnumerableExtensions_ForEach_Asyncの実験");

            var array = Enumerable.Range(0, 10).ToArray();
            Func<int, Task> func = async i =>
            {
                await Task.Delay(100);
                Console.WriteLine($"< {i} >");
            };

            Console.WriteLine();
            Console.WriteLine("すべての要素に、非同期処理を並行して実行したい場合");
            Console.WriteLine("実行順は不定だが速い（すべてのCPUを用いて、空いたCPUから順に実行される）");
            Console.WriteLine("処理順とか関係ない場合用");
            await Task.WhenAll(array.Select(func));

            Console.WriteLine();
            Console.WriteLine("すべての要素に、非同期処理を順番に実行したい場合");
            Console.WriteLine("1個のCPUを用いて実行されるため、実行順が固定で遅い");
            Console.WriteLine("処理順が重要で、それぞれ同時に実行することもできない場合用");
            await array.ForEach(func);

            Console.WriteLine();
            Console.WriteLine("そもそも同期処理を並行して行いたい場合とか、他にもいろんな方法があるよ");
        }

        #endregion

        private static async Task SemaphoreExtensionsの実験()
        {
            Console.WriteLine();
            Console.WriteLine("########## ########## ##########");
            Console.WriteLine("SemaphoreExtensionsの実験");

            {
                Console.WriteLine();
                Console.WriteLine("排他処理しない場合");
                var tasks = Enumerable.Range(0, 5).Select(t => Task.Run(async () =>
                {
                    Console.Write($"{t} -> ");
                    await Task.Delay(100);
                    Console.WriteLine($"<- {t}");
                }));

                await Task.WhenAll(tasks);
            }

            {
                Console.WriteLine();
                Console.WriteLine("排他処理する場合");
                var s = new SemaphoreSlim(1);
                var tasks = Enumerable.Range(1, 5).Select(t => Task.Run(async () =>
                {
                    await s.Critical(async () =>
                    {
                        Console.Write($"{t} -> ");
                        await Task.Delay(10);
                        Console.WriteLine($"<- {t}");
                    });
                }));

                await Task.WhenAll(tasks);
            }
        }
    }
}
