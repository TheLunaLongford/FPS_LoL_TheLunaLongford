using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_manager : MonoBehaviour
{
    public GameObject pose;

    private ui_pose_script ui_Pose_Script;
    // Start is called before the first frame update
    void Start()
    {
        ui_Pose_Script = pose.GetComponent<ui_pose_script>();
    }

    public void update_pose(int pose)
    {
        // Poses: 0 -> Normal   0 -> Crouch   2 -> Floor
        ui_Pose_Script.update_pose(pose);
    }
}
