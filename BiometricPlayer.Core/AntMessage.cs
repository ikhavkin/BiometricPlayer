using System.Collections.Generic;
using System.Linq;

namespace BiometricPlayer.Core
{
    /// <summary>
    /// ANT Message.
    /// </summary>
    /// <remarks>Immutable object.</remarks>
    public class AntMessage
    {
        /// <summary>
        /// Constructs <see cref="AntMessage"/>.
        /// </summary>
        /// <param name="messageId">Message ID.</param>
        /// <param name="data">Message data.</param>
        public AntMessage(byte messageId, IEnumerable<byte> data = null)
        {
            MessageId = messageId;
            Data = (data ?? Enumerable.Empty<byte>()).ToList().AsReadOnly();
        }

        /// <summary>
        /// Message ID.
        /// </summary>
        public byte MessageId { get; private set; }

        /// <summary>
        /// Message data.
        /// </summary>
        public IList<byte> Data
        {
            get; private set;
        }
    }
}