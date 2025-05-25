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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
