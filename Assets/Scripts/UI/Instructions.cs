using UnityEngine;

public class Instructions : MonoBehaviour
{
    public KeyCode key = KeyCode.H;
    private void Update()
    {
        if (Input.GetKeyDown(key)) gameObject.SetActive(false);
    }
}