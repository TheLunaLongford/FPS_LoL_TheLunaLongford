using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_controller : MonoBehaviour
{

    private CharacterController character_controller;
    private float rotation_speed;
    public bool inverted_axis;

    private float vertical_rotation;
    private float vertical_rotation_limit;


    [SerializeField] private float normal_speed;
    [SerializeField] private float crouch_speed;
    [SerializeField] private float floor_speed;

    [SerializeField] private float nomral_camera_height = 0.8f;
    [SerializeField] private float crouch_camera_height = 0.3f;
    [SerializeField] private float floor_camera_height = 0.1f;

    [SerializeField] private bool is_crouch = false;
    [SerializeField] private bool is_floor = false;

    public ui_manager ui_manager;

    void Start()
    {
        character_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        rotation_speed = 2.0f;
        inverted_axis = false;
        vertical_rotation = 0.0f;
        vertical_rotation_limit = 45.0f;

        // Initialize 3-states values
        // Normal
        normal_speed = 5.0f;
        nomral_camera_height = 0.8f;
        // Crouch
        crouch_speed = 2.5f;
        crouch_camera_height = 0.3f;
        // Floor
        floor_speed = 1.0f;
        floor_camera_height = 0.1f;

        // Initialize position flags
        is_crouch = false;
        is_floor = false;
    }

    void Update()
    {
        float vertical;
        float horizontal;

        float actual_speed;
        if (is_crouch)
            actual_speed = crouch_speed;
        else if(is_floor)
            actual_speed = floor_speed;
        else
            actual_speed = normal_speed;

        vertical = Input.GetAxis("Vertical") * actual_speed;
        horizontal = Input.GetAxis("Horizontal") * actual_speed;

        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        character_controller.SimpleMove(movement);

        change_pose();

        // Camera
        float mouseX = Input.GetAxis("Mouse X") * rotation_speed;
        float mouseY = Input.GetAxis("Mouse Y") * rotation_speed;

        if (inverted_axis)
        {
            mouseY *= -1f;
        }

        // Aplicar rotacion vertical con limitacion
        vertical_rotation -= mouseY;
        vertical_rotation = Mathf.Clamp(vertical_rotation, -vertical_rotation_limit, vertical_rotation_limit);

        Camera.main.transform.localRotation = Quaternion.Euler(vertical_rotation, 0, 0);
        transform.Rotate(0, mouseX, 0);
    }

    private void change_pose()
    {
        // Poses: 0 -> Normal   0 -> Crouch   2 -> Floor
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!is_crouch && !is_floor)
            {
                is_crouch = true;
                ui_manager.update_pose(1);
                Camera.main.transform.localPosition = new Vector3(0, crouch_camera_height, 0);
            }
            else if (is_crouch)
            {
                is_crouch = false;
                is_floor = true;
                ui_manager.update_pose(2);
                Camera.main.transform.localPosition = new Vector3(0, floor_camera_height, 0);
            }
            else if (is_floor)
            {
                is_floor = false;
                ui_manager.update_pose(0);
                Camera.main.transform.localPosition = new Vector3(0, nomral_camera_height, 0);
            }
        }
    }
}
