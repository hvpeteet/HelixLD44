using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Plant
{
    public override bool IsHabitable(Tile t)
    {
        return t.plant.GetOrganismType() == OrganismType.Barren;
    }

    public override void OnGrownUp()
    {
        Debug.Log("Grass grew up");
    }

    public override OrganismType GetOrganismType()
    {
        return OrganismType.Grass;
    }
}
