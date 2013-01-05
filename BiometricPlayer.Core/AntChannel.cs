using System;
using System.Reactive.Linq;
using ANT_Managed_Library;

namespace BiometricPlayer.Core
{
    /// <summary>
    /// Channel of the <see cref="AntDevice"/>.
    /// </summary>
    public class AntChannel : IAntChannel
    {
        ANT_Channel channel;

        /// <summary>
        /// Constructs <see cref="AntChannel"/>.
        /// </summary>
        /// <param name="channel">Underlying <see cref="ANT_Channel"/> object from official ANT managed API.</param>
        internal AntChannel(ANT_Channel channel)
        {
            this.channel = channel;
        }

        /// <summary>
        /// Messages recieved by this channel.
        /// </summary>
        public IObservable<AntMessage> Events
        {
            get
            {
                // todo: implement
                return Observable.Never<AntMessage>();
            }
        }
    }
}
