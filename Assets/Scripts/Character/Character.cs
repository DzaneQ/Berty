using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    protected int strength;
    protected int power;
    protected int dexterity;
    protected int health;
    public int Strength { get => strength; }
    public int Power { get => power; }
    public int Dexterity { get => dexterity; }
    public int Health { get => health; }

    protected List<int[]> blockRange = new List<int[]>();
    protected List<int[]> riposteRange = new List<int[]>();
    protected List<int[]> attackRange = new List<int[]>();

    public bool CanBlock(int[] source)
    {
        foreach (int[] block in blockRange)
            if (block == source) return true;
        return false;
    }

    public bool CanRiposte(int[] source)
    {
        foreach (int[] riposteField in blockRange)
            if (riposteField == source) return true;
        return false;
    }

    protected void InitiateStats(int str, int pwr, int dex, int hp)
    {
        strength = str;
        power = pwr;
        dexterity = dex;
        health = hp;
    }
    protected void InitiateRange(int[] relativeCoordinate, List<int[]> range)
    {
        if (relativeCoordinate.Length != 2) throw new System.Exception("Wrong dimension of coordinates.");
        if (range.Contains(relativeCoordinate)) throw new System.Exception("Duplicate coordinates.");
        range.Add(relativeCoordinate);
    }
}
