using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Msgs;
using RosMessageTypes.Tf2;
using System.Linq;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;

public class TF_Frame : MonoBehaviour
{
    public string frame_name;
    public TF_Frame[] childs;

    public void GetFrames(TF_Frame parent_frame,TF_Frame current_frame,ref List<TransformStampedMsg> frames,bool initial_flag = false)
    {
        if(!initial_flag)
        {
            Vector3 translation = current_frame.transform.position-parent_frame.transform.position;
            translation = Quaternion.Inverse(parent_frame.transform.rotation) * translation;
            Quaternion rotation = Quaternion.Inverse(parent_frame.transform.rotation) * current_frame.transform.rotation;


            TransformStampedMsg transformStamped = new TransformStampedMsg
            {
                header = new HeaderMsg
                {
                    frame_id = parent_frame.frame_name,  
                    stamp = new TimeMsg
                    {
                        sec = (uint)Time.time,
                        nanosec = (uint)((Time.time - (int)Time.time) * 1e9)
                    }
                },
                child_frame_id = current_frame.frame_name,  
                transform = new TransformMsg
                {
                    translation = new Vector3Msg(translation.x, translation.z, translation.y),
                    rotation = new QuaternionMsg(-rotation.x, -rotation.z, -rotation.y, rotation.w)
                }
            };

            frames.Add(transformStamped);
        }

        foreach(TF_Frame frame in current_frame.childs)
        {
            GetFrames(current_frame,frame,ref frames);
        }
    }
}
