namespace Screenbits.Unity
{
    /// <summary>
    /// Represents the current state of Screenbits recording.
    /// </summary>
    public enum ScreenbitsRecordingState
    {
        /// <summary>
        /// Not recording.
        /// </summary>
        Idle,

        /// <summary>
        /// Recording is in progress.
        /// </summary>
        Recording,

        /// <summary>
        /// Recording is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Recording is starting up.
        /// </summary>
        Starting,

        /// <summary>
        /// Recording is stopping.
        /// </summary>
        Stopping
    }
}
