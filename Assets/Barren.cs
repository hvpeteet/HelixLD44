using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barren : Plant
{
    public override bool IsHabitable(Tile t)
    {
        return false;
    }

    public override void OnGrownUp()
    {
        Debug.Log("The Barrens cannot grow, this should never happen");
    }

    public override OrganismType GetOrganismType()
    {
        return OrganismType.Barren;
    }
}