using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Point hex_coords;
    public Plant plant;
    //public Animal prey;
    //public Animal predator;

    public void AddOrganism(Plant plant)
    {
        this.plant = plant;
    }
}
