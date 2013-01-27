using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using Autofac.Core;
using BiometricPlayer.Core;
using FluentAssertions;
using Machine.Specifications;
using System.Reactive.Linq;

namespace BiometricPlayer.Tests.Integration
{
    [Subject("Test code to probe live Garmin HRM sensor data")]
    public class When_configured_device_for_recieving_Garmin_HRM_data : DeviceSpec
    {
        static IAntChannel channel;

        Establish context = () =>
                                {
                                    device.NetworkKey = new byte[] {0xb9, 0xa5, 0x21, 0xfb, 0xbd, 0x72, 0xc3, 0x45};
                                    device.Init();
                                    channel = device.Channel;
                                };

        Because of = () =>
                         {
                             channel.ChannelPeriod = 0x1f86;
                             channel.ChannelRFFrequency = 0x39;

                             channel.Open();
                         };

        It should_not_throw = () => { };

        It should_raise_channel_events = () =>
                                             {
                                                 var events = channel.Events.Take(40).ToEnumerable();
                                                 string[] dataAsStrings =
                                                     events.Select(
                                                         message => BitConverter.ToString(message.Data.ToArray()))
                                                           .ToArray();
                                                 dataAsStrings.Should().HaveCount(40);
                                             };
    }

    [Subject(typeof (MockAntChannel))]
    public class When_listening_hrm_channel_for_65_messages : MockHrmChannelSpec
    {
        static IEnumerable<AntMessage> messages;

        Because of = () =>
            messages = channel.Events.Take(65).TakeUntil(DateTimeOffset.Now + TimeSpan.FromSeconds(2)).ToEnumerable();

        It should_not_fail = () => { };

        It should_return_65_messages = () =>
            messages.Should().HaveCount(65);

        It should_contain_alternating_highest_bit_of_page_byte = () =>
            {
                var pageHighestBytes = messages.Select(m => m.Data[1] & 0x80); // todo: check why it's on the index 1
                pageHighestBytes.Should().Contain(0x00).And.Contain(0x80);
            };
    }

    public class MockHrmChannelSpec
    {
        protected static IAntChannel channel;

        Establish context = () =>
                                {
                                    channel = new MockAntChannel();
                                };
    }

    public class MockAntChannel : IAntChannel
    {
        public static readonly TimeSpan Period = TimeSpan.FromSeconds(0.25); //TimeSpan.FromTicks(10000000L * 8070 / 32768);

        static readonly byte[][] Messages = new[]
            {
                new byte[] {0x00, 0x04, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39},
                new byte[] {0x00, 0x04, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39},
                new byte[] {0x00, 0x04, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39},
                new byte[] {0x00, 0x04, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39},

                new byte[] {0x00, 0x84, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39},
                new byte[] {0x00, 0x84, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39},
                new byte[] {0x00, 0x84, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39},
                new byte[] {0x00, 0x84, 0x00, 0x72, 0x35, 0xB0, 0x39, 0x12, 0x39}
            };

        public IObservable<AntMessage> Events
        {
            get
            {
                var messagesLoop = Messages.ToObservable().Repeat();

//                return messagesLoop.Zip(Observable.Interval(Period)).
//                    Select(t => new AntMessage(0x4e, t.Item1));
                return messagesLoop.Zip(Observable.Interval(Period), (bytes, i) => bytes).
                    Select(bytes => new AntMessage(0x4e, bytes));
//                return Observable.Interval(Period).Select(i => new AntMessage(0x4e, Messages[i % Messages.Length]));
            }
        }

        public byte ChannelRFFrequency { get; set; }

        public ushort ChannelPeriod { get; set; }

        public ushort DeviceNumber { get; set; }

        public void Open()
        {
        }
    }

    /*
     Example output:
     dataAsStrings
{string[40]}
    [0]: "00-04-00-72-35-B0-39-12-39"
    [1]: "00-04-00-B0-39-C2-3D-13-39"
    [2]: "00-84-00-B0-39-C2-3D-13-39"
    [3]: "00-84-00-B0-39-C2-3D-13-39"
    [4]: "00-84-00-B0-39-C2-3D-13-39"
    [5]: "00-84-00-C2-3D-E7-41-14-39"
    [6]: "00-04-00-C2-3D-E7-41-14-39"
    [7]: "00-04-00-C2-3D-E7-41-14-39"
    [8]: "00-04-00-C2-3D-E7-41-14-39"
    [9]: "00-04-00-E7-41-AF-45-15-3A"
    [10]: "00-84-00-E7-41-AF-45-15-3A"
    [11]: "00-84-00-E7-41-AF-45-15-3A"
    [12]: "00-84-00-E7-41-AF-45-15-3A"
    [13]: "00-84-00-E7-41-AF-45-15-3A"
    [14]: "00-04-00-E7-41-AF-45-15-3A"
    [15]: "00-04-00-E7-41-AF-45-15-3A"
    [16]: "00-04-00-E7-41-AF-45-15-3A"
    [17]: "00-04-00-AF-45-F9-4C-16-3A"
    [18]: "00-84-00-AF-45-F9-4C-16-3A"
    [19]: "00-84-00-AF-45-F9-4C-16-3A"
    [20]: "00-84-00-AF-45-F9-4C-16-3A"
    [21]: "00-84-00-AF-45-F9-4C-16-3A"
    [22]: "00-04-00-AF-45-F9-4C-16-3A"
    [23]: "00-04-00-AF-45-F9-4C-16-3A"
    [24]: "00-04-00-AF-45-F9-4C-16-3A"
    [25]: "00-04-00-F9-4C-44-55-17-3A"
    [26]: "00-84-00-F9-4C-44-55-17-3A"
    [27]: "00-84-00-F9-4C-44-55-17-3A"
    [28]: "00-81-37-24-12-44-55-17-3A"
    [29]: "00-84-00-44-55-3D-59-18-3A"
    [30]: "00-04-00-44-55-3D-59-18-3A"
    [31]: "00-04-00-44-55-3D-59-18-3A"
    [32]: "00-04-00-44-55-3D-59-18-3A"
    [33]: "00-04-00-3D-59-51-5D-19-3A"
    [34]: "00-84-00-3D-59-51-5D-19-3A"
    [35]: "00-84-00-3D-59-51-5D-19-3A"
    [36]: "00-84-00-3D-59-51-5D-19-3A"
    [37]: "00-84-00-51-5D-8A-61-1A-3A"
    [38]: "00-04-00-51-5D-8A-61-1A-3A"
    [39]: "00-04-00-51-5D-8A-61-1A-3A"
     */
}
