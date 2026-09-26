using UnityEngine;

public class HeldBarVisualizer : MonoBehaviour
{
    [Header("Sprite References")]
    [SerializeField] private SpriteRenderer barSprite;
    [SerializeField] private SpriteRenderer leftHandSprite;
    [SerializeField] private SpriteRenderer rightHandSprite;
    
    [Header("Initial Offsets")]
    [SerializeField] private Vector3 initialOffset;
    [SerializeField] private float initialRotationOffset;

    void OnValidate()
    {
        // Calculate Initial Offsets
        initialOffset = barSprite.transform.position - CalculateTargetPosition();
        initialRotationOffset = barSprite.transform.rotation.eulerAngles.z - CalculateTargetAngle();
    }

    void Update()
    {
        if (leftHandSprite == null || rightHandSprite == null)
            return;

        UpdateSprites();
    }

    private void UpdateSprites()
    {
        // Update Bar Position and Rotation
        float targetAngle = CalculateTargetAngle();
        barSprite.transform.position = CalculateTargetPosition() + initialOffset;
        barSprite.transform.rotation = Quaternion.AngleAxis(targetAngle + initialRotationOffset, Vector3.forward);
        
        // Update Hand Rotations
        leftHandSprite.transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        rightHandSprite.transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
    }

    #region Bar Helpers

    private Vector3 CalculateTargetPosition()
    {
        return (leftHandSprite.transform.position + rightHandSprite.transform.position) / 2;
    }

    private float CalculateTargetAngle()
    {
        Vector3 direction = rightHandSprite.transform.position - leftHandSprite.transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    #endregion
}