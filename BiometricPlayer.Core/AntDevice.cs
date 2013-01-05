using System;
using ANT_Managed_Library;

namespace BiometricPlayer.Core
{
    public class AntDevice : IAntDevice
    {
        private ANT_Device device;
        private bool isDisposed = false;

        public void Init()
        {
            device = new ANT_Device();
        }

        public void Reset()
        {
            if (device == null)
            {
                throw new InvalidOperationException("Device is not initialized.");
            }
        }

        public byte[] NetworkKey { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(typeof(AntDevice).FullName);
            }

            isDisposed = true;
        }
    }
}