using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using BiometricPlayer.Core;
using BiometricPlayer.Tests.Integration;
using FakeItEasy;
using FluentAssertions;
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

    [Subject(typeof(AntChannel))]
    public class When_ANT_channel_event_is_raised : MockedChannelSpec
    {
        static IObservable<AntMessage> events;
        static IEnumerable<AntMessage> occurredEvents;

        Establish context = () =>
            {
                events = channel.Events;
            };

        Because of = () =>
            occurredEvents = events.ToEnumerable();

        It should_notify_about_new_message = () =>
            occurredEvents.Should().Equal(testMessage);
    }

    public class ChannelSpec : DeviceSpec
    {
        protected static IAntChannel channel;

        Establish context = () =>
            {
                device.Init();
                channel = device.Channel;
            };
    }

    public class MockedChannelSpec : DeviceSpec
    {
        protected static IAntDevice device;
        protected static IAntChannel channel;
        protected static Action act;
        protected static AntMessage testMessage;

        Establish context = () =>
        {
            channel = A.Fake<IAntChannel>();
            testMessage = new AntMessage(0x01);
            A.CallTo(() => channel.Events).Returns(Observable.Return(testMessage));

            device = A.Fake<AntDevice>();
            A.CallTo(() => device.Channel).Returns(channel);

            device.Init();
        };

        Cleanup all = () =>
            device.Dispose();
    }
}