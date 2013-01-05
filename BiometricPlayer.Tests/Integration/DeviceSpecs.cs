using System;
using BiometricPlayer.Core;
using FluentAssertions;
using Machine.Specifications;

namespace BiometricPlayer.Tests.Integration
{
    [Subject(typeof(AntDevice))]
    public class When_device_is_reset
    {
        static IAntDevice device;

        Establish context =
            () =>
                {
                    device = new AntDevice();
                    device.Init();
                };

        Because of = () => device.Reset();
        
        It should_not_throw = () => { };

        Cleanup all = () =>
            device.Dispose();
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_reset_without_init
    {
        static IAntDevice device;
        static Action act;

        Establish context = () =>
            device = new AntDevice();

        Because of = () => act = () =>
            device.Reset();

        It should_throw_invalid_operation_exception = () => 
            act.ShouldThrow<InvalidOperationException>();

        Cleanup all = () =>
            device.Dispose();
    }


    [Subject(typeof(AntDevice))]
    public class When_device_is_disposed
    {
        static AntDevice device;

        Establish context = () =>
            device = new AntDevice();

        Because of = () => device.Dispose();

        It should_not_throw = () => { };

        Cleanup all = () =>
            device.Dispose();
    }    
    
    [Subject(typeof(AntDevice))]
    public class When_device_is_disposed_twice
    {
        static IAntDevice device;
        static Action act;

        Establish context = () =>
            device = new AntDevice();

        Because of = () => act = () =>
            {
                device.Dispose();
                device.Dispose();
            };

        It should_throw_object_disposed_exception = () => 
            act.ShouldThrow<ObjectDisposedException>().
            WithMessage("AntDevice", ComparisonMode.Substring);

        Cleanup all = () =>
            device.Dispose();
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_initialized
    {
        static AntDevice device;

        Establish context = () =>
            {
                device = new AntDevice
                             {
                                 NetworkKey = new byte[] {0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8}
                             };
            };

        Because of = () =>
            device.Init();

        It should_not_throw = () => { };
        It should_have_network_key_set = () =>
            device.NetworkKey.Should().Equal(new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 });

        Cleanup all = () =>
            device.Dispose();
    }
}
