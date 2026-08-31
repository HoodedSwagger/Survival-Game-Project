#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Encel4dus
{
    [InitializeOnLoad]
    public static class VertexColorDebugger
    {
        private static SceneView _currentSceneView;
        private static Shader _vertexColorShader;
        private static Shader _vertexColorRShader;
        private static Shader _vertexColorGShader;
        private static Shader _vertexColorBShader;

        static VertexColorDebugger()
        {
            SceneView.AddCameraMode("Vertex RGB", "Vertex Colors");
            SceneView.AddCameraMode("Vertex R", "Vertex Colors");
            SceneView.AddCameraMode("Vertex G", "Vertex Colors");
            SceneView.AddCameraMode("Vertex B", "Vertex Colors");

            EditorApplication.update += OnEditorUpdate;
        }

        private static void OnEditorUpdate()
        {
            if (SceneView.lastActiveSceneView != _currentSceneView)
            {
                if (_currentSceneView != null)
                {
                    _currentSceneView.onCameraModeChanged -= OnCameraModeChanged;
                }

                if (SceneView.lastActiveSceneView != null)
                {
                    _currentSceneView = SceneView.lastActiveSceneView;
                    _currentSceneView.onCameraModeChanged += OnCameraModeChanged;
                }
            }
        }

        private static void OnCameraModeChanged(SceneView.CameraMode mode)
        {
            switch (mode.name)
            {
                case "Vertex RGB":
                    EnableVertexColorMode("VertexColorRGB", ref _vertexColorShader);
                    break;
                case "Vertex R":
                    EnableVertexColorMode("VertexColorR", ref _vertexColorRShader);
                    break;
                case "Vertex G":
                    EnableVertexColorMode("VertexColorG", ref _vertexColorGShader);
                    break;
                case "Vertex B":
                    EnableVertexColorMode("VertexColorB", ref _vertexColorBShader);
                    break;
                default:
                    DisableVertexColorMode();
                    break;
            }
        }

        private static void EnableVertexColorMode(string shaderName, ref Shader shader)
        {
            if (shader == null)
            {
                string[] guids = AssetDatabase.FindAssets(shaderName + " t:Shader");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
                }
            }

            if (_currentSceneView != null && shader != null)
            {
                _currentSceneView.SetSceneViewShaderReplace(shader, "RenderType");
            }
            else
            {
                Debug.LogWarning("Failed to apply VertexColor shader: SceneView or shader is null.");
            }
        }

        private static void DisableVertexColorMode()
        {
            if (_currentSceneView != null)
            {
                _currentSceneView.SetSceneViewShaderReplace(null, null);
            }
        }
    }
}

#endif