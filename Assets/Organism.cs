using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Organism : MonoBehaviour
{
    public int turns_remaining_as_baby;
    public int reproduction_cooldown;
    public Point location;

    public abstract void OnGrownUp();
    public abstract bool IsHabitable(Tile t);
    public abstract OrganismType GetOrganismType();
}
