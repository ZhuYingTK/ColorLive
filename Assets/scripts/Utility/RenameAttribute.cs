using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class RenameAttribute : PropertyAttribute
{
    public string name;

    public RenameAttribute(string name)
    {
        this.name = name;
    }
}