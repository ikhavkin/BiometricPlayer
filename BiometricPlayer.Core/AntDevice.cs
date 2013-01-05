using System;

namespace BiometricPlayer.Core
{
    public class AntDevice : IAntDevice
    {
        private bool isDisposed = false;

        public void Init()
        {
        }

        public void Reset()
        {
        }

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