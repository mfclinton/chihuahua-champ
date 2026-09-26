using System;
using UnityEngine;

[Serializable]
public class DogJointVisualizerEntry
{
    [Header("References")]
    public Transform jointTransform;
    
    public Transform startTransform;
    public Transform endTransform;

    [Header("Animation")]
    [Range(0, 1)] public float animStartProgressThreshold = 0f;
    [Range(0, 1)] public float animEndProgressThreshold = 1f;

    private float animInterpolationSpeed = 2f;
    
    [Header("Shakiness")]
    public float shakiness = 0.1f;
    
    // Internal Variables
    private float animProgress;
    
    public void Initialize()
    {
        
    }

    public void UpdateJoinTransform(float progress)
    {
        animProgress = Mathf.Lerp(animProgress, progress, animInterpolationSpeed * Time.deltaTime);
        float t = Mathf.InverseLerp(animStartProgressThreshold, animEndProgressThreshold, animProgress);
        UpdatePosition(t);
        UpdateRotation(t);
    }
        
    private void UpdatePosition(float t)
    {
        Vector3 targetPosition = Vector3.Lerp(startTransform.position, endTransform.position, t);

        float x = Mathf.PerlinNoise(0f, Time.time);
        float y = Mathf.PerlinNoise(100f, Time.time);

        Vector3 shake = new Vector2(x, y) * shakiness;
        jointTransform.position = targetPosition + shake;
    }
    
    private void UpdateRotation(float t)
    {
        Quaternion targetRotation = Quaternion.Lerp(startTransform.rotation, endTransform.rotation, t);
        
        jointTransform.rotation = targetRotation;
    }
}