using System;
using System.Linq;
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
                device.NetworkKey = new byte[] { 0xb9, 0xa5, 0x21, 0xfb, 0xbd, 0x72, 0xc3, 0x45 };
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
                                                    events.Select(message => BitConverter.ToString(message.Data.ToArray())).ToArray();
                                                dataAsStrings.Should().HaveCount(40);
                                            };
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