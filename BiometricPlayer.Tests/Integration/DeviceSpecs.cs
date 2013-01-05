using System;
using BiometricPlayer.Core;
using FluentAssertions;
using Machine.Specifications;

namespace BiometricPlayer.Tests.Integration
{
    [Subject(typeof(AntDevice))]
    public class When_device_is_reset : DeviceSpec
    {
        Establish context = () => 
            device.Init();

        Because of = () => device.Reset();
        
        It should_not_throw = () => { };
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_reset_without_init : DeviceSpec
    {
        Because of = () => act = () =>
            device.Reset();

        It should_throw_invalid_operation_exception = () => 
            act.ShouldThrow<InvalidOperationException>();
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_disposed : DeviceSpec
    {
        Because of = () =>
            device.Dispose();

        It should_not_throw = () => { };
    }    
    
    [Subject(typeof(AntDevice))]
    public class When_device_is_disposed_twice : DeviceSpec
    {
        Because of = () => act = () =>
            {
                device.Dispose();
                device.Dispose();
            };

        It should_throw_object_disposed_exception = () => 
            act.ShouldThrow<ObjectDisposedException>().
            WithMessage("AntDevice", ComparisonMode.Substring);
    }

    [Subject(typeof(AntDevice))]
    public class When_disposed_device_is_initialized : DeviceSpec
    {
        Establish context = () =>
            device.Dispose();

        Because of = () => act = () =>
            device.Init();

        It should_throw_object_disposed_exception = () => 
            act.ShouldThrow<ObjectDisposedException>();
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_initialized : DeviceSpec
    {
        Because of = () =>
            device.Init();

        It should_not_throw = () => { };
        It should_have_default_network_key_set = () =>
            device.NetworkKey.Should().Equal(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_initialized_with_key : DeviceSpec
    {
        Establish context = () =>
            device.NetworkKey = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 };

        Because of = () =>
            device.Init();

        It should_not_throw = () => { };
        It should_have_configured_network_key_set = () =>
            device.NetworkKey.Should().Equal(new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 });
    }

    [Subject(typeof(AntDevice))]
    public class When_set_network_key_and_modify_it : DeviceSpec
    {
        Establish context = () =>
            {
                key = new byte[] {0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8};
                device.NetworkKey = key;
            };

        Because of = () =>
            key[2] = 0xff;

        It should_have_original_key = () =>
            device.NetworkKey.Should().Equal(new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 });

        static byte[] key;
    }

    [Subject(typeof(AntDevice))]
    public class When_network_key_is_retrieved_and_modified : DeviceSpec
    {
        Establish context = () =>
            device.NetworkKey = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 };

        Because of = () =>
            device.NetworkKey[2] = 0xff;

        It should_have_original_key = () =>
            device.NetworkKey.Should().Equal(new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 });

        static byte[] key;
    }

    public class When_network_key_with_length_other_than_8_is_set : DeviceSpec
    {
        Because of = () => act = () =>
            device.NetworkKey = new byte[] { 0x1, 0x2, 0x3 };

        It should_throw_invalid_argument_exception = () =>
            act.ShouldThrow<ArgumentException>().
                And.ParamName.Should().Be("value");
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_initialized_and_its_key_changes : DeviceSpec
    {
        Establish context = () =>
            device.Init();

        Because of = () => act = () =>
            device.NetworkKey = new byte[] {0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8};

        It should_throw_invalid_operation_exception = () =>
            act.ShouldThrow<InvalidOperationException>();
    }

    public class When_channel_is_requested_on_uninitialized_device : DeviceSpec
    {
        static AntChannel channel;

        Because of = () => act = () =>
            channel = device.Channel;

        It should_throw_invalid_operation_exception = () =>
            act.ShouldThrow<InvalidOperationException>();
    }

    public class When_channel_is_requested_on_initialized_device : DeviceSpec
    {
        static AntChannel channel;

        Establish context = () =>
            device.Init();

        Because of = () =>
            channel = device.Channel;

        It should_return_channel = () =>
            channel.Should().NotBeNull();
    }

    public class DeviceSpec
    {
        protected static AntDevice device;
        protected static Action act;

        Establish context = () =>
            device = new AntDevice();

        Cleanup all = () =>
            device.Dispose();
    }
}
