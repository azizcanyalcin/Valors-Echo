using UnityEngine;
public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    public virtual void ExecuteEffect(Transform transform)
    {
        Debug.Log("Effect executed !!!");
    }
}