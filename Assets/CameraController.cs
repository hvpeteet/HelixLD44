using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float DEADZONE_MIN = -0.005f;
    private const float DEADZONE_MAX = 0.005f;
    private const float MASTER_DEADZONE = 0.005f;
    private const float MOUSE_DEADZONE = 0.8f;
    public Vector3 min_corner = new Vector3(-1, 1, -1);
    public Vector3 max_corner = new Vector3(7, 5, 7);
    private const float ZOOM_SPEED = 200.0f;
    private const float MOUSE_MOVE_SPEED = 5.0f;

    private void Update()
    {
        Vector3 desired_delta = new Vector3(0, 0, 0);

        float scroll_input = Input.GetAxis("Mouse ScrollWheel");
        if (scroll_input < DEADZONE_MIN || scroll_input > DEADZONE_MAX)
        {
            desired_delta += scroll_input * ZOOM_SPEED * Time.deltaTime * gameObject.transform.forward;
        }

        // We interpret how close to the side of the screen the mouse is as an input.
        // [-1 (left) to +1 (right)]
        float mouse_horizontal_input = (Input.mousePosition.x * 2 - Screen.width) / Screen.width;

        if (Mathf.Abs(mouse_horizontal_input) > MOUSE_DEADZONE)
        {
            // 0 to 1 based on how close to the edge of the screen they are within the triggering region.
            float unweighted_magnitude = (Mathf.Abs(mouse_horizontal_input) - MASTER_DEADZONE) / (1.0f - MASTER_DEADZONE);
            desired_delta += new Vector3(unweighted_magnitude * MOUSE_MOVE_SPEED * Mathf.Sign(mouse_horizontal_input) * Time.deltaTime, 0, 0);
        }

        // [-1 (down) to +1 (up)]
        float mouse_vertical_input = (Input.mousePosition.y * 2 - Screen.height) / Screen.height;

        if (Mathf.Abs(mouse_vertical_input) > MOUSE_DEADZONE)
        {
            // 0 to 1 based on how close to the edge of the screen they are within the triggering region.
            float unweighted_magnitude = (Mathf.Abs(mouse_vertical_input) - MASTER_DEADZONE) / (1.0f - MASTER_DEADZONE);
            desired_delta += new Vector3(0, 0, unweighted_magnitude * MOUSE_MOVE_SPEED * Mathf.Sign(mouse_vertical_input) * Time.deltaTime);
        }

        if (desired_delta.magnitude > MASTER_DEADZONE)
        {
            Vector3 target_position = gameObject.transform.position + desired_delta;
            Vector3 clamped_position = new Vector3(
                Mathf.Clamp(target_position.x, min_corner.x, max_corner.x),
                Mathf.Clamp(target_position.y, min_corner.y, max_corner.y),
                Mathf.Clamp(target_position.z, min_corner.z, max_corner.z));
            Vector3 clamped_delta = (target_position - clamped_position);
            Vector3 clamped_delta_percent = new Vector3(
                clamped_delta.x / desired_delta.x,
                clamped_delta.y / desired_delta.y,
                clamped_delta.z / desired_delta.z);
            // Zero out entries where desired_delta is zero to avoid NAN values 
            // from making it past this point.
            if (desired_delta.x == 0)
            {
                clamped_delta_percent.x = 0;
            }
            if (desired_delta.y == 0)
            {
                clamped_delta_percent.y = 0;
            }
            if (desired_delta.z == 0)
            {
                clamped_delta_percent.z = 0;
            }
            float max_clamp_percent = Mathf.Max(
                clamped_delta_percent.x, 
                clamped_delta_percent.y, 
                clamped_delta_percent.z);
            Vector3 final_delta = desired_delta * (1.0f - max_clamp_percent);
            gameObject.transform.Translate(final_delta, Space.World);
        }
    }
}
