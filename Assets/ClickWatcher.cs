using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickWatcher : MonoBehaviour
{
    void Update()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Constants.TILES_LAYER_MASK))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Point coord = hit.collider.gameObject.GetComponent<Tile>().hex_coords;
                Debug.Log(string.Format("({0}, {1})", coord.x, coord.y));
                // Highlight the selected tile.
                // Enable the UI for that tile.
            }
        }
    }
}
