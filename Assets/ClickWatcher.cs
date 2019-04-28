using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickWatcher : MonoBehaviour
{
    public GameMutator mutator;
    public Material highlight_mat;
    private List<MeshRenderer> to_revert = new List<MeshRenderer>();
    private List<Material> revert_to = new List<Material>();

    void Update()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Constants.TILES_LAYER_MASK))
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Clear old highlights.
                for (int i = 0; i < to_revert.Count; i++)
                {
                    to_revert[i].material = revert_to[i];
                }
                to_revert.Clear();
                revert_to.Clear();

                // Get the surrounding tiles and highlight them.
                Point coord = hit.collider.gameObject.GetComponent<Tile>().hex_coords;
                Debug.Log(string.Format("Clicked tile ({0}, {1})", coord.x, coord.y));
                List<Tile> to_highlight = mutator.GetNeighbors(hit.collider.gameObject.GetComponent<Tile>());
                foreach (Tile t in to_highlight)
                {
                    MeshRenderer renderer = t.plant.gameObject.GetComponentInChildren<MeshRenderer>();
                    to_revert.Add(renderer);
                    revert_to.Add(renderer.material);
                    renderer.material = highlight_mat;
                }
                // Enable the UI for that tile.
            }
        }
    }
}
