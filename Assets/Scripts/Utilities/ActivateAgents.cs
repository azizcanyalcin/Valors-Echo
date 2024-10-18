using System.Collections;
using UnityEngine;

public class ActivateAgents : MonoBehaviour
{
    [SerializeField] private GameObject agent;
    [SerializeField] private string tagName;
    [SerializeField] private float delay;
    [HideInInspector] public bool isAgentActivated = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(tagName))
        {
            StartCoroutine("SetAgentActive");
        }
    }

    private IEnumerator SetAgentActive()
    {
        isAgentActivated = true;
        yield return new WaitForSeconds(delay);
        if (agent) agent.SetActive(true);
        Destroy(gameObject);
    }
}