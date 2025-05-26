using System;
using UnityEngine;

[CreateAssetMenu]
public class lootsystem : ScriptableObject
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
