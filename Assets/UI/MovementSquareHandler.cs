using UnityEngine;

public class MovementSquareHandler : MonoBehaviour
{
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(
            Screen.width * PlayerController.MOUSE_CURSOR_THRESHOLD * 2,
            Screen.height * PlayerController.MOUSE_CURSOR_THRESHOLD * 2
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
