using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelBoundaryHandler : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner;
    [SerializeField] private PolygonCollider2D defaultBoundaries;
    [SerializeField] private PolygonCollider2D newBoundaries;
    [SerializeField] private SceneTransitionUI transition;
    private Player player;
    private float delay = 2f;
    private void Start()
    {
        player = PlayerManager.instance.player;
    }
    public void RemoveConfinerBounds()
    {
        if (cinemachineConfiner != null)
        {
            cinemachineConfiner.m_BoundingShape2D = null;
            cinemachineConfiner.InvalidateCache();
        }
    }

    public void SetConfinerBounds(PolygonCollider2D newBoundingShape)
    {
        if (cinemachineConfiner != null && newBoundaries)
        {
            cinemachineConfiner.m_BoundingShape2D = newBoundingShape;
            cinemachineConfiner.InvalidateCache();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine("SceneTransition", delay);

        }
    }
    public void AssignNewBounds(PolygonCollider2D newBounds)
    {
        SetConfinerBounds(newBounds);
    }

    IEnumerator SceneTransition(float delay)
    {
        defaultBoundaries.isTrigger = false;
        transition.FadeOut();

        player.spriteRenderer.color = Color.clear;
        player.isPlayerActive = false;
        player.SetVelocityToZero();
        player.stateMachine.ChangeState(player.idleState);
        
        yield return new WaitForSeconds(delay);
        player.spriteRenderer.color = Color.white;
        RemoveConfinerBounds();
        if (newBoundaries)
            SetConfinerBounds(newBoundaries);

        transition.FadeIn();

        player.isPlayerActive = true;
    }
}