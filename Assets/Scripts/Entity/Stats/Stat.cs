using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }
    public void SetValue(int value)
    {
        baseValue = value;
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }
    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }

}