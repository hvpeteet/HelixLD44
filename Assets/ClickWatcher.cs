using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickWatcher : MonoBehaviour
{
    public GameMutator mutator;
    public Material highlight_mat;
    private List<Tile> highlighted = new List<Tile>();
    private List<Material> highlighted_old_mats = new List<Material>();
    private Tile selected;

    void ClearHighlights()
    {
        // Clear old highlights.
        for (int i = 0; i < highlighted.Count; i++)
        {
            highlighted[i].plant.gameObject.GetComponentInChildren<MeshRenderer>().material = highlighted_old_mats[i];
        }
        highlighted.Clear();
        highlighted_old_mats.Clear();
    }

    void Update()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, Constants.TILES_LAYER_MASK))
        {
            Tile clicked_tile = hit.collider.gameObject.GetComponent<Tile>();
            Point coord = clicked_tile.hex_coords;
            Debug.Log(string.Format("Clicked tile ({0}, {1})", coord.x, coord.y));

            if (selected == null || !highlighted.Contains(clicked_tile))
            {
                ClearHighlights();

                // Get the surrounding tiles and highlight them.
                List<Tile> to_highlight = mutator.GetNeighbors(hit.collider.gameObject.GetComponent<Tile>());
                foreach (Tile t in to_highlight)
                {
                    if (clicked_tile.plant.IsHabitable(t))
                    {
                        MeshRenderer renderer = t.plant.gameObject.GetComponentInChildren<MeshRenderer>();
                        highlighted.Add(t);
                        highlighted_old_mats.Add(renderer.material);
                        renderer.material = highlight_mat;
                    }
                }
                selected = clicked_tile;
            } else if (highlighted.Contains(clicked_tile))
            {
                ClearHighlights();
                mutator.SetOrganism(selected.plant.GetOrganismType(), clicked_tile.GetComponent<Tile>().hex_coords);
                selected = null;
            }

                
            // Enable the UI for that tile.
        }
    }
}
