using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : Plant
{
    public override bool IsHabitable(Tile t)
    {
        return t.plant.GetOrganismType() == OrganismType.Bushes;
    }

    public override void OnGrownUp()
    {
        Debug.Log("Trees grew up");
    }

    public override OrganismType GetOrganismType()
    {
        return OrganismType.Trees;
    }
}