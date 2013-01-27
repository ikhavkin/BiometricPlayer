using System.Reactive.Concurrency;
using BiometricPlayer.Core;
using Microsoft.Reactive.Testing;

namespace BiometricPlayer.Tests
{
    /// <summary>
    /// Test scheduler provider with <see cref="TestScheduler"/> instance for each scheduler type.
    /// </summary>
    public class TestSchedulerProvider : ISchedulerProvider
    {
        readonly IScheduler currentThreadProvider = new TestScheduler();
        readonly IScheduler dispatcherProvider = new TestScheduler();
        readonly IScheduler immediateProvider = new TestScheduler();
        readonly IScheduler newThreadProvider = new TestScheduler();
        readonly IScheduler threadPoolProvider = new TestScheduler();
        readonly IScheduler taskPoolProvider = new TestScheduler();

        IScheduler ISchedulerProvider.CurrentThread
        {
            get { return currentThreadProvider; }
        }

        IScheduler ISchedulerProvider.Dispatcher
        {
            get { return dispatcherProvider; }
        }

        IScheduler ISchedulerProvider.Immediate
        {
            get { return immediateProvider; }
        }

        IScheduler ISchedulerProvider.NewThread
        {
            get { return newThreadProvider; }
        }

        IScheduler ISchedulerProvider.ThreadPool
        {
            get { return threadPoolProvider; }
        }

        IScheduler ISchedulerProvider.TaskPool
        {
            get { return taskPoolProvider; }
        }
    }
}