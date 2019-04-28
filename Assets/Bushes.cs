using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : Plant
{
    public override bool IsHabitable(Tile t)
    {
        return t.plant.GetOrganismType() == OrganismType.Grass;
    }

    public override void OnGrownUp()
    {
        Debug.Log("Bushes grew up");
    }

    public override OrganismType GetOrganismType()
    {
        return OrganismType.Bushes;
    }
}