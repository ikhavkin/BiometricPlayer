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
        public AntMessage(byte messageId)
        {
            MessageId = messageId;
        }

        /// <summary>
        /// Message ID.
        /// </summary>
        public byte MessageId { get; private set; }
    }
}