using UnityEngine;
public enum CursorState
{
    Default,
    Menu,
    Aim
}
public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D menuCursor;
    public Texture2D aimCursor;

    public Vector2 defaultHotspot = Vector2.zero;
    public Vector2 menuHotspot = Vector2.zero;
    public Vector2 aimHotspot = Vector2.zero;

    private CursorState currentCursorState;

    private void Start()
    {
        SetCursorState(CursorState.Default);
    }

    public void SetCursorState(CursorState state)
    {
        // Check if the cursor state is changing
        if (state == currentCursorState) return;

        currentCursorState = state;
        Texture2D cursorTexture = defaultCursor;
        Vector2 hotspot = defaultHotspot;

        // Select the cursor texture and hotspot based on the state
        switch (state)
        {
            case CursorState.Default:
                cursorTexture = defaultCursor;
                hotspot = defaultHotspot;
                break;
            case CursorState.Menu:
                cursorTexture = menuCursor;
                hotspot = menuHotspot;
                break;
            case CursorState.Aim:
                cursorTexture = aimCursor;
                hotspot = aimHotspot;
                break;
        }

        // Set the cursor with the chosen texture and hotspot
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
