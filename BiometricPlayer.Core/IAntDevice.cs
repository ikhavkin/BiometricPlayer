using System;

namespace BiometricPlayer.Core
{
    public interface IAntDevice : IDisposable
    {
        void Init();
        void Reset();
    }
}