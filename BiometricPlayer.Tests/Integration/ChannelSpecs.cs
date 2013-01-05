using System;
using System.Reactive.Concurrency;
using BiometricPlayer.Core;
using BiometricPlayer.Tests.Integration;
using Machine.Specifications;
using System.Reactive.Linq;

namespace Namespace
{
    [Subject(typeof(AntChannel))]
    public class When_subscribing_to_channel_events : ChannelSpec
    {
        static IObservable<AntMessage> events;
        static IDisposable subscription;

        Establish context = () => 
            events = channel.Events;

        Because of = () =>
            subscription = events.SubscribeOn(NewThreadScheduler.Default).ObserveOn(NewThreadScheduler.Default).Subscribe();

        It should_not_throw = () => { };

        Cleanup all = () =>
            subscription.Dispose();
    }

    public class ChannelSpec : DeviceSpec
    {
        protected static AntChannel channel;

        Establish context = () =>
            {
                device.Init();
                channel = device.Channel;
            };
    }
}