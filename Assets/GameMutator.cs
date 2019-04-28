using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class GameMutator : MonoBehaviour
{
    private GameState game_state;

    public GameObject barren_prefab;
    public GameObject tile_prefab;
    public GameObject grass_prefab;
    public GameObject bushes_prefab;
    public GameObject trees_preab;
    // TODO Prey and predator prefab fields.

    private void Start()
    {
        InitMap();
    }

    public void InitMap()
    {
        game_state = new GameState();
        InitBarrenMap(10, 10);
        game_state.life_force = Constants.LIFE_FORCE_CAPACITY;
        game_state.turn = 1;

        // Set up the starting level layout.
        SetOrganism(OrganismType.Trees, new Point(0, 0));

        SetOrganism(OrganismType.Bushes, new Point(0, 1));
        SetOrganism(OrganismType.Bushes, new Point(1, 0));
        SetOrganism(OrganismType.Bushes, new Point(1, 1));

        SetOrganism(OrganismType.Grass, new Point(0, 2));
        SetOrganism(OrganismType.Grass, new Point(1, 2));
        SetOrganism(OrganismType.Grass, new Point(2, 0));
        SetOrganism(OrganismType.Grass, new Point(2, 1));
        SetOrganism(OrganismType.Grass, new Point(2, 2));
    }

    public void SetOrganism(OrganismType type, Point point)
    {
        GameObject prefab;
        switch(type)
        {
            case OrganismType.Barren:
                prefab = barren_prefab;
                break;
            case OrganismType.Grass:
                prefab = grass_prefab;
                break;
            case OrganismType.Bushes:
                prefab = bushes_prefab;
                break;
            case OrganismType.Trees:
                prefab = trees_preab;
                break;
            // TODO: Prey and predator.
            default:
                throw new System.InvalidOperationException(string.Format("Cannot create adult organism of type {0}", type));
        }
        GameObject instance = Instantiate(prefab, Helpers.PointToHexLocation(point), Quaternion.identity);
        if (instance.GetComponent<Plant>() != null)
        {
            if (game_state.map[point.x, point.y].plant != null)
            {
                Destroy(game_state.map[point.x, point.y].plant.gameObject);
            }
            game_state.map[point.x, point.y].plant = instance.GetComponent<Plant>();
        }
        // TODO: Similar for animals.
        
        //Debug.Log(instance.GetComponent<Plant>());
    }

    public void EndTurn()
    {
        // Tick every organism from the bottom of the foodchain up.
        // Assumes no organism's Tick() depends on organisms in adjacent tiles.
        foreach (Tile t in game_state.map)
        {
            if (t.plant != null)
            {
                Tick(t.plant);
            }
            /*
            if (t.prey != null)
            {
                Tick(t.prey);
            }
            if (t.predator != null)
            {
                Tick(t.predator);
            }
            */
        }
        game_state.life_force = Constants.LIFE_FORCE_CAPACITY;
        // TODO: Night day cycle.
        // TODO: Reload UI.
        game_state.turn++;
    }

    // Moves time forward 1 tick for the organism.
    // This grows babies or reduces cooldowns.
    public void Tick(Organism organism)
    {
        // TODO: Move to accelerate.
        /*
        if (game_state.life_force <= 0)
        {
            throw new System.InvalidOperationException("Life force already at zero, cannot accelerate growth.");
        }
        */
        
        if (organism.turns_remaining_as_baby > 0)
        {
            organism.turns_remaining_as_baby--;
            if (organism.turns_remaining_as_baby == 0)
            {
                organism.OnGrownUp();
            }
        } else if (organism.reproduction_cooldown > 0)
        {
            organism.reproduction_cooldown--;
        }

        // TODO: Move to accelerate;
        /*
        else
        {
            throw new System.InvalidOperationException(string.Format("There is nothing to accelerate for {0}", organism));
        }
        game_state.life_force--;
        */
    }

    // TODO
    //public void Move(Animal animal, Tile tile) {}

    public void Reproduce<T>(T organism, Tile tile) where T : Plant //, Animal, Prey
    {
        if (organism.IsHabitable(tile))
        {
            tile.AddOrganism(organism);
        }
    }

    // Please don't use this to change any game state. 
    // C# is bad about returning read only information.
    public GameState GetGameState()
    {
        return game_state;
    }

    private void InitBarrenMap(int width, int height)
    {
        game_state.map = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                game_state.map[x, y] = Instantiate(tile_prefab, Helpers.PointToHexLocation(new Point(x, y)), Quaternion.identity).GetComponent<Tile>();
                game_state.map[x, y].hex_coords = new Point(x, y);
                SetOrganism(OrganismType.Barren, new Point(x, y));
            }
        }
    }

    public List<Tile> GetNeighbors(Tile t)
    {
        List<Tile> neighbors = new List<Tile>();
        bool is_even = (t.hex_coords.x % 2 == 0);
        int[,] offsets = is_even ? Constants.offset_directions_even : Constants.offset_directions_odd;
        for (int offset_index = 0; offset_index < offsets.GetLength(0); offset_index++)
        {
            Point combined = new Point(t.hex_coords.x + offsets[offset_index, 0], t.hex_coords.y + offsets[offset_index, 1]);
            if (combined.x >= game_state.map.GetLength(0) || 
                combined.y >= game_state.map.GetLength(1) || 
                combined.x < 0 ||
                combined.y < 0)
            {
                continue;
            }
            neighbors.Add(game_state.map[combined.x, combined.y]);
        }
        return neighbors;
    }
}
