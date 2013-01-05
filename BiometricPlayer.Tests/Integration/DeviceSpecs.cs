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

        Establish context = () =>
            device = new AntDevice();

        Because of = () => device.Reset();
        
        It should_not_throw = () => { };
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_disposed
    {
        static AntDevice device;

        Establish context = () =>
            device = new AntDevice();

        Because of = () => device.Dispose();

        It should_not_throw = () => { };
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
    }

    [Subject(typeof(AntDevice))]
    public class When_device_is_initialized
    {
        static IAntDevice device;

        Establish context = () =>
            device = new AntDevice();

        Because of = () => device.Init();

        It should_not_throw = () => { };
    }
}