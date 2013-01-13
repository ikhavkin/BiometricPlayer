using System;
using BiometricPlayer.Core;
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
        It should_raise_channel_event = () => 
            channel.Events.FirstAsync().Timeout(TimeSpan.FromSeconds(10)).Wait();
    }
}