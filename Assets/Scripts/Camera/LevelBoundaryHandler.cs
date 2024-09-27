using Cinemachine;
using UnityEngine;
public class LevelBoundaryHandler : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner;
    [SerializeField] private PolygonCollider2D defaultBoundaries;
    public SceneTransition transition;

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
            RemoveConfinerBounds();
            transition.FadeOut();
        }
    }
    public void AssignNewBounds(PolygonCollider2D newBounds)
    {
        SetConfinerBounds(newBounds);
    }
}