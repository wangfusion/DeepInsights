using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public static class TaskCombinatorExtensions
    {
        public async static Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            Task winner = await (Task.WhenAny(task, Task.Delay(timeout)));
            if (winner != task) throw new TimeoutException();
            return await task;
        }

        public static Task<TResult> WithCancellation<TResult>(this Task<TResult> task, CancellationToken cancelToken)
        {
            var tcs = new TaskCompletionSource<TResult>();
            var reg = cancelToken.Register(() => tcs.TrySetCanceled());
            task.ContinueWith(ant =>
            {
                reg.Dispose();
                if (ant.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (ant.IsFaulted)
                {
                    tcs.TrySetException(ant.Exception.InnerException);
                }
                else
                {
                    tcs.TrySetResult(ant.Result);
                }
            });

            return tcs.Task;
        }
    }
}
