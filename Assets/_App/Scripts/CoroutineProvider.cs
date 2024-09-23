using UnityEngine;

public class CoroutineProvider : MonoBehaviour
{
    public static CoroutineProvider Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
