using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class ScreenShake : MonoBehaviour
{

    private Vector3 originalPos;
    private float timeAtCurrentFrame;
    private float timeAtLastFrame;
    private float fakeDelta;

    private void Update()
    {
        // Calculate a fake delta time, so we can Shake while game is paused.
        timeAtCurrentFrame = Time.realtimeSinceStartup;
        fakeDelta = timeAtCurrentFrame - timeAtLastFrame;
        timeAtLastFrame = timeAtCurrentFrame;
    }

    public void Shake(float duration, float amount)
    {
        originalPos = gameObject.transform.localPosition;
        StopAllCoroutines();
        StartCoroutine(CShake(duration, amount));
    }

    private IEnumerator CShake(float duration, float intensity)
    {
        float endTime = Time.time + duration;
        while (duration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * intensity;

            duration -= fakeDelta;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScreenShake))]
public class ScreenShakeInspector : Editor
{
    private float duration = 1f;
    private float intensity = 0.1f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Shake (play mode)"))
        {
            ScreenShake targetScreen = target as ScreenShake;
            targetScreen.Shake(duration, intensity);
        }
        GUILayout.Label("duration", GUILayout.ExpandWidth(false));
        duration = EditorGUILayout.FloatField(duration);
        GUILayout.Label("intensity", GUILayout.ExpandWidth(false));
        intensity = EditorGUILayout.FloatField(intensity);

        GUILayout.EndHorizontal();

    }
}
#endif
