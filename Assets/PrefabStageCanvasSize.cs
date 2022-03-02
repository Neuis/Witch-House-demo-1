using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;

/// <summary>
/// Watches for state changes of prefab stage.
/// Adds special component to the root game object when prefab gets open.
/// See related thread:
/// http://forum.unity.com/threads/any-way-to-set-canvas-settings-for-prefab-mode.569923/#post-5159192
/// </summary>
[InitializeOnLoad]
static class PrefabStageCanvasSizeWatcher
{
    static PrefabStageCanvasSizeWatcher()
    {
        if (EditorApplication.isPlaying)
            return;

        PrefabStage.prefabStageOpened += stage =>
        {
            // get environment components
            var envRT = stage.prefabContentsRoot.transform.parent as RectTransform;
            var envCanvas = envRT?.GetComponent<Canvas>();
            if (envRT == null || envCanvas == null)
                return; // non UI prefab

            // add resizer component to root game object of prefab
            var resizer = stage.prefabContentsRoot.GetComponent<PrefabStageCanvasSize>()
                        ?? stage.prefabContentsRoot.AddComponent<PrefabStageCanvasSize>();

            resizer.Init(envRT, envCanvas);
        };
    }
}
#endif

/// <summary>
/// Changes environment canvas size according to chosen mode in the inspector.
/// </summary>
[ExecuteAlways]
public class PrefabStageCanvasSize : MonoBehaviour
{
    // change default values if needed
    public Vector2 ReferenceSize = new Vector2(1920, 1080);
    public Mode CanvasSizeMode = Mode.ExpandUsingGameViewAspect;

    public enum Mode
    {
        ReferenceSize,
        HeightFollowsGameViewAspect,
        WidthFollowsGameViewAspect,
        ExpandUsingGameViewAspect,
        NativeBehaviour
    }

#if UNITY_EDITOR
    // components of environment game object
    private RectTransform _envRect;
    private Canvas _envCanvas;
    private bool _isInitialized;
    private Vector2 _lastSize;
    private Mode _lastMode;

    public void Init(RectTransform envRect, Canvas envCanvas)
    {
        _envRect = envRect;
        _envCanvas = envCanvas;
        _isInitialized = true;

        Resize();
    }

    public void Resize()
    {
        ReferenceSize.x = Math.Max(ReferenceSize.x, 10);
        ReferenceSize.y = Math.Max(ReferenceSize.y, 10);

        _lastMode = CanvasSizeMode;
        _lastSize = ReferenceSize;

        if (CanvasSizeMode == Mode.NativeBehaviour)
        {
            _envCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _envRect.sizeDelta = Handles.GetMainGameViewSize();
        }
        else
        if (CanvasSizeMode == Mode.ReferenceSize)
        {
            _envCanvas.renderMode = RenderMode.WorldSpace;
            _envRect.sizeDelta = ReferenceSize;
        }
        else
        {
            var game = Handles.GetMainGameViewSize();
            var size = ReferenceSize;

            switch (CanvasSizeMode)
            {
                case Mode.ExpandUsingGameViewAspect:
                    {
                        float refAspect = size.x / size.y;
                        float gameAspect = game.x / game.y;

                        if (refAspect < gameAspect)
                            size.x *= gameAspect / refAspect;
                        else
                            size.y *= refAspect / gameAspect;
                    }
                    break;

                case Mode.HeightFollowsGameViewAspect:
                    size.y = size.x * game.y / game.x;
                    break;

                case Mode.WidthFollowsGameViewAspect:
                    size.x = size.y * game.x / game.y;
                    break;

                default:
                    throw new NotImplementedException(CanvasSizeMode.ToString());
            }

            _envCanvas.renderMode = RenderMode.WorldSpace;
            _envRect.sizeDelta = size;
        }
    }

    private void Update()
    {
        if (!_isInitialized)
            return;

        bool mustResize = CanvasSizeMode == Mode.ExpandUsingGameViewAspect
                        || CanvasSizeMode == Mode.HeightFollowsGameViewAspect
                        || CanvasSizeMode == Mode.WidthFollowsGameViewAspect
                        || _lastMode != CanvasSizeMode
                        || _lastSize != ReferenceSize;

        if (mustResize)
            Resize();
    }
#endif
}
