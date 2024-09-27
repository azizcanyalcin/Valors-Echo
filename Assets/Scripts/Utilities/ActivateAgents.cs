using UnityEngine;

public class ActivateAgents : MonoBehaviour
{
    [SerializeField] private GameObject agent;
    [SerializeField] private string tagName;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag(tagName))
        {
            agent.SetActive(true);
        }
    }
}