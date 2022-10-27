using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chess.Game;

public class CameraController : MonoBehaviour
{
    public Vector3 focal_point = new Vector3(4, 4, 4); //center of the board
    public float rotation_speed = 1f; //Mouse sensitivity multiplier
    public float zoom_min = 5f; //minimum zoom distance
    public float zoom_max = 20f; //maximum zoom distance
    public float zoom_speed = 1f; //Multiplayer of scroll wheel input

    private void Start() {
        // set camera to look at focal point
        transform.LookAt(focal_point);
    }

    // Update is called once per frame
    void Update()
    {
        if(!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.LeftApple)) {
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        } else {
            DimensionScroll(Input.GetAxis("Mouse ScrollWheel"));
        }
        if(!Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0)) {
            CamOrbit(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        } else if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0)) {
            CamPan(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        transform.LookAt(focal_point);
    }

    void DimensionScroll(float scroll) {
        if(scroll > 0) {
            //scroll up
            GameManager.Instance.boardInterface.CycleFourthDimension(1);
        } else if (scroll < 0) {
            //scroll down
            GameManager.Instance.boardInterface.CycleFourthDimension(-1);
        }
    }

    void CamPan(float x, float y) {
        // move camera left/right and up/down
        Vector3 translation = new Vector3(-x, -y, 0) * rotation_speed;
        transform.Translate(translation);
        focal_point += translation;
    }

    private void Zoom(float zoom_diff) {
        // zoom in or out
        if(zoom_diff != 0) {
            Vector3 zoom_direction = transform.position - focal_point;
            float zoom_distance = zoom_direction.magnitude;
            zoom_direction.Normalize();
            zoom_distance -= zoom_diff * zoom_speed;
            zoom_distance = Mathf.Clamp(zoom_distance, zoom_min, zoom_max);
            transform.position = focal_point + zoom_direction * zoom_distance;
        }
    }

    private void CamOrbit(float vertical_displacement, float horizontal_displacement) {
        // rotate camera around focal point
        transform.RotateAround(focal_point, Vector3.up, horizontal_displacement * rotation_speed);
        // Get normalized perpendicular vector to camera and focal point
        Vector3 perpendicular = Vector3.Cross(transform.position - focal_point, Vector3.up).normalized;
        transform.RotateAround(focal_point, perpendicular, -vertical_displacement * rotation_speed);
    }
}
