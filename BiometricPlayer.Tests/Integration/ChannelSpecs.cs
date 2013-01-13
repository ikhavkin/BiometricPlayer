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
    public class When_channel_event_is_raised : MockedChannelSpec
    {
        static IObservable<AntMessage> events;
        static IEnumerable<AntMessage> occurredEvents;

        Establish context = () =>
            {
                channel.Open();
                events = channel.Events;
            };

        Because of = () =>
            occurredEvents = events.ToEnumerable();

        It should_notify_about_new_message = () =>
            occurredEvents.Should().Equal(testMessage);
    }

    public class When_channel_is_opened_twice : ChannelSpec
    {
        Establish context = () =>
            channel.Open();

        Because of = () => act = () =>
            channel.Open();

        It should_throw_invalid_operation_exception = () =>
            act.ShouldThrow<InvalidOperationException>();
    }

    public class When_channel_is_fully_configured_and_opened : ChannelSpec
    {
        Establish context = () =>
            {
                channel.ChannelRFFrequency = 1;
                channel.DeviceNumber = 2;
                channel.ChannelPeriod = 3;
            };

        Because of = () => act = () =>
            channel.Open();

        It should_not_throw = () => { };
    }

    public class When_frequency_is_set_on_opened_channel : ChannelSpec
    {
        Establish context = () =>
            channel.Open();

        Because of = () => act = () =>
            channel.ChannelRFFrequency = 123;

        It should_throw_invalid_operation_exception = () =>
            act.ShouldThrow<InvalidOperationException>();
    }

    public class When_device_num_is_set_on_opened_channel : ChannelSpec
    {
        Establish context = () =>
            channel.Open();

        Because of = () => act = () =>
            channel.DeviceNumber = 123;

        It should_throw_invalid_operation_exception = () =>
            act.ShouldThrow<InvalidOperationException>();
    }

    public class When_channel_period_is_set_on_opened_channel : ChannelSpec
    {
        Establish context = () =>
            channel.Open();

        Because of = () => act = () =>
            channel.ChannelPeriod = 123;

        It should_throw_invalid_operation_exception = () =>
            act.ShouldThrow<InvalidOperationException>();
    }

    // todo: ensure you can't change state of the channel after its device is disposed

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