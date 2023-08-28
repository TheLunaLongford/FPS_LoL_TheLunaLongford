using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_pose_script : MonoBehaviour
{
    private int pose_status;
    public Image img_actual;
    public Sprite img_normal;
    public Sprite img_crouch;
    public Sprite img_floor;
    void Start()
    {
        pose_status = 0;
        update_pose(pose_status);
    }

    public void update_pose(int pose_number)
    {
        // Poses: 0 -> Normal   0 -> Crouch   2 -> Floor    
        switch (pose_number)
        {
            // Normal
            case 0:
                img_actual.sprite = img_normal;
                break;
            // Crouch
            case 1:
                img_actual.sprite = img_crouch;
                break;
            // Normal
            case 2:
                img_actual.sprite = img_floor;
                break;
        }
    }
}
