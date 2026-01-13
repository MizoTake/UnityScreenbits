using UnityEngine;
using Screenbits.Unity;

namespace Screenbits.Samples
{
    /// <summary>
    /// Simple example showing basic Screenbits usage.
    /// Press R to Start/Stop recording, P to Pause/Resume.
    /// </summary>
    public class ScreenbitsSimpleExample : MonoBehaviour
    {
        private ScreenbitsController _controller;

        private void Start()
        {
            _controller = ScreenbitsController.Instance;
            _controller.OnStateChanged += state => Debug.Log($"Recording state: {state}");

            Debug.Log("Screenbits Example - Press R to Record/Stop, P to Pause/Resume");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                _controller.ToggleRecording();

            if (Input.GetKeyDown(KeyCode.P))
                _controller.TogglePause();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 250, 150));
            GUILayout.Label($"State: {_controller.CurrentState}");
            GUILayout.Label($"Screenbits Running: {_controller.IsScreenbitsRunning()}");
            GUILayout.Space(10);

            if (GUILayout.Button(_controller.IsRecording ? "Stop (R)" : "Record (R)"))
                _controller.ToggleRecording();

            GUI.enabled = _controller.IsRecording;
            string pauseLabel = _controller.CurrentState == ScreenbitsRecordingState.Paused ? "Resume (P)" : "Pause (P)";
            if (GUILayout.Button(pauseLabel))
                _controller.TogglePause();
            GUI.enabled = true;

            GUILayout.EndArea();
        }
    }
}
