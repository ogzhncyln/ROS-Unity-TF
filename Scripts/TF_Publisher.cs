using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Msgs;
using RosMessageTypes.Tf2;
using RosMessageTypes.Geometry;

[RequireComponent(typeof(TF_Frame))]
public class TF_Publisher : MonoBehaviour
{
    public float freq = 10f;  
    
    private ROSConnection ros;
    private TF_Frame frame;
    private float timeSinceLastPublish = 0f;

    void Start()
    {
        frame = GetComponent<TF_Frame>();
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TFMessageMsg>("/tf", 10);
    }

    void FixedUpdate()
    {
        timeSinceLastPublish += Time.fixedDeltaTime;

        if (timeSinceLastPublish >= 1f / freq)
        {
            PublishMessage();
            timeSinceLastPublish = 0f;  
        }
    }

    void PublishMessage()
    {
        List<TransformStampedMsg> frames = new List<TransformStampedMsg>();
        frame.GetFrames(frame,frame,ref frames,true);
        
        TFMessageMsg tfMessage = new TFMessageMsg();
        tfMessage.transforms = frames.ToArray();

        ros.Publish("/tf",tfMessage);
    }
}
