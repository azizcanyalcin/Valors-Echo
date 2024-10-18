using UnityEngine;

public class ActivateUI : MonoBehaviour
{
    [SerializeField] GameObject intructionsUI;
    private ActivateAgents activateAgents;
    private void Start()
    {
        activateAgents = GetComponent<ActivateAgents>();
    }
    private void Update()
    {
        if (activateAgents.isAgentActivated)
        {
            intructionsUI.SetActive(true);
            Destroy(this);
        }
    }

}