using System;
using System.Diagnostics;
using UnityEngine;

namespace Screenbits.Unity
{
    /// <summary>
    /// Simple controller for Screenbits screen recorder.
    /// Uses URI scheme to control Microsoft Store version of Screenbits.
    /// </summary>
    public class ScreenbitsController : MonoBehaviour
    {
        private const string ProcessName = "Screenbits";
        private const string UriLaunch = "screenbits://";
        private const string UriStart = "screenbits://start";
        private const string UriStartBackground = "screenbits://start/?background";
        private const string UriStop = "screenbits://stop";
        private const string UriPause = "screenbits://pause";
        private const string UriResume = "screenbits://resume";

        private static ScreenbitsController _instance;
        private ScreenbitsRecordingState _currentState = ScreenbitsRecordingState.Idle;

        [SerializeField]
        private bool _backgroundMode = true;

        [SerializeField]
        private bool _enableDebugLog = true;

        /// <summary>
        /// Singleton instance of ScreenbitsController.
        /// </summary>
        public static ScreenbitsController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ScreenbitsController>();
                    if (_instance == null)
                    {
                        var go = new GameObject("ScreenbitsController");
                        _instance = go.AddComponent<ScreenbitsController>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Current recording state.
        /// </summary>
        public ScreenbitsRecordingState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    LogDebug($"State: {value}");
                    OnStateChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// Whether recording is active (Recording or Paused).
        /// </summary>
        public bool IsRecording => _currentState == ScreenbitsRecordingState.Recording ||
                                   _currentState == ScreenbitsRecordingState.Paused;

        /// <summary>
        /// Use background mode (no Screenbits UI window).
        /// </summary>
        public bool BackgroundMode
        {
            get => _backgroundMode;
            set => _backgroundMode = value;
        }

        /// <summary>
        /// Event fired when recording state changes.
        /// </summary>
        public event Action<ScreenbitsRecordingState> OnStateChanged;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        /// <summary>
        /// Checks if Screenbits process is running.
        /// </summary>
        public bool IsScreenbitsRunning()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            try
            {
                var processes = Process.GetProcessesByName(ProcessName);
                bool running = processes.Length > 0;
                foreach (var p in processes) p.Dispose();
                return running;
            }
            catch (Exception ex)
            {
                LogDebug($"Process check failed: {ex.Message}");
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// Launches Screenbits application.
        /// </summary>
        /// <returns>True if launch command was sent.</returns>
        public bool Launch()
        {
            if (IsScreenbitsRunning())
            {
                LogDebug("Screenbits is already running");
                return true;
            }

            LogDebug("Launching Screenbits...");
            return ExecuteUri(UriLaunch);
        }

        /// <summary>
        /// Starts recording.
        /// </summary>
        /// <param name="background">Override background mode setting.</param>
        public void StartRecording(bool? background = null)
        {
            if (CurrentState == ScreenbitsRecordingState.Recording)
            {
                LogDebug("Already recording");
                return;
            }

            bool useBackground = background ?? _backgroundMode;
            string uri = useBackground ? UriStartBackground : UriStart;

            CurrentState = ScreenbitsRecordingState.Starting;
            ExecuteUri(uri);
            CurrentState = ScreenbitsRecordingState.Recording;
        }

        /// <summary>
        /// Stops recording.
        /// </summary>
        public void StopRecording()
        {
            if (CurrentState == ScreenbitsRecordingState.Idle)
            {
                LogDebug("Not recording");
                return;
            }

            CurrentState = ScreenbitsRecordingState.Stopping;
            ExecuteUri(UriStop);
            CurrentState = ScreenbitsRecordingState.Idle;
        }

        /// <summary>
        /// Pauses recording.
        /// </summary>
        public void PauseRecording()
        {
            if (CurrentState != ScreenbitsRecordingState.Recording)
            {
                LogDebug($"Cannot pause in state {CurrentState}");
                return;
            }

            ExecuteUri(UriPause);
            CurrentState = ScreenbitsRecordingState.Paused;
        }

        /// <summary>
        /// Resumes paused recording.
        /// </summary>
        public void ResumeRecording()
        {
            if (CurrentState != ScreenbitsRecordingState.Paused)
            {
                LogDebug($"Cannot resume in state {CurrentState}");
                return;
            }

            ExecuteUri(UriResume);
            CurrentState = ScreenbitsRecordingState.Recording;
        }

        /// <summary>
        /// Toggles recording (Start/Stop).
        /// </summary>
        public void ToggleRecording()
        {
            if (IsRecording)
                StopRecording();
            else
                StartRecording();
        }

        /// <summary>
        /// Toggles pause (Pause/Resume).
        /// </summary>
        public void TogglePause()
        {
            if (CurrentState == ScreenbitsRecordingState.Recording)
                PauseRecording();
            else if (CurrentState == ScreenbitsRecordingState.Paused)
                ResumeRecording();
        }

        private bool ExecuteUri(string uri)
        {
            LogDebug($"Execute: {uri}");

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = uri,
                    UseShellExecute = true,
                    CreateNoWindow = true
                });
                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[Screenbits] Failed: {uri}\n{ex.Message}");
                return false;
            }
#else
            UnityEngine.Debug.LogWarning("[Screenbits] Windows only");
            return false;
#endif
        }

        private void LogDebug(string message)
        {
            if (_enableDebugLog)
            {
                UnityEngine.Debug.Log($"[Screenbits] {message}");
            }
        }
    }
}
