using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelBoundaryHandler : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner;
    [SerializeField] private PolygonCollider2D defaultBoundaries;
    [SerializeField] private SceneTransition transition;
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
        if (cinemachineConfiner != null)
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
        transition.FadeOut();

        player.isPlayerActive = false;
        player.SetVelocityToZero();

        yield return new WaitForSeconds(delay);

        RemoveConfinerBounds();

        transition.FadeIn();

        player.isPlayerActive = true;
    }
}