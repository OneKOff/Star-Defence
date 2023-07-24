using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private Canvas canvasGUI;
    [SerializeField] private Canvas canvasStatic;
    [SerializeField] private Canvas canvasWorld;

    public Canvas CanvasGUI => canvasGUI;
    public Canvas CanvasStatic => canvasStatic;
    public Canvas CanvasWorld => canvasWorld;

    private void InitializeInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Awake()
    {
        InitializeInstance();
    }
}
