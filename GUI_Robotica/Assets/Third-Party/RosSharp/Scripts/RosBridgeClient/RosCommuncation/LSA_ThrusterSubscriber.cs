
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class LSA_ThrusterSubscriber : Subscriber<Messages.lsa_auv_msgs.actuators_msgs.Thruster>
    {

		private float speedRPM;
        private bool isMessageReceived;

        protected override void Start()
        {
			base.Start();
        }
        private void FixedUpdate()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(Messages.lsa_auv_msgs.actuators_msgs.Thruster message)
        {
            speedRPM = message.speed_rpm;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
			Debug.Log("Speed RPM: " + speedRPM);
            isMessageReceived = false;
        }

    }
}

