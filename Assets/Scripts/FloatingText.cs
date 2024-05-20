using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [HideInInspector]
    public TextMeshPro textComponent;
    // Start is called before the first frame update
    void Awake()
    {
        textComponent = GetComponent<TextMeshPro>();
    }
}
