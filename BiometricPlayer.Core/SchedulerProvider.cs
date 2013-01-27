using System.Reactive.Concurrency;

namespace BiometricPlayer.Core
{
    /// <summary>
    /// Provider of schedulers.
    /// </summary>
    public interface ISchedulerProvider
    {
        IScheduler CurrentThread { get; }
        IScheduler Dispatcher { get; }
        IScheduler Immediate { get; }
        IScheduler NewThread { get; }
        IScheduler ThreadPool { get; } 
        IScheduler TaskPool { get; }  
    }

    /// <summary>
    /// Default scheduler provider that resolves to the real schedulers.
    /// </summary>
    public class SchedulerProvider : ISchedulerProvider
    {
        public IScheduler CurrentThread
        {
            get { return Scheduler.CurrentThread; }
        }

        public IScheduler Dispatcher
        {
            get { return DefaultScheduler.Instance; }
        }

        public IScheduler Immediate 
        {
            get { return Scheduler.Immediate; }
        }

        public IScheduler NewThread
        {
            get { return NewThreadScheduler.Default; }
        }

        public IScheduler ThreadPool
        {
            get { return ThreadPoolScheduler.Instance; }
        }

        public IScheduler TaskPool
        {
            get { return TaskPoolScheduler.Default; }
        }
    }
}
