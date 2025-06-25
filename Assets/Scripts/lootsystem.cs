using System;
using UnityEngine;

[CreateAssetMenu]
public class lootsystem : ScriptableObject // so we can create object with this type
{
    public Sprite lootsprite;
    public String lootname;
    public float dropchance;


    public lootsystem(String lootname, float dropchance)
    {
        this.lootname = lootname;
        this.dropchance = dropchance;
    }
    

}
