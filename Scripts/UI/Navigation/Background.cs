using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private BackgroundType type;
    public BackgroundType Type => type;
}
