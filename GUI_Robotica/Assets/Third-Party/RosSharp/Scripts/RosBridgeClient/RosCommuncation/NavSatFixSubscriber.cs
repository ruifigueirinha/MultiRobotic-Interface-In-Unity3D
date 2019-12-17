
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class NavSatFixSubscriber : Subscriber<Messages.Sensor.NavSatFix>
    {
        private float latitude;
        private float longitude;
        private float altitude;
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

        protected override void ReceiveMessage(Messages.Sensor.NavSatFix message)
        {
            latitude = message.latitude;
            longitude = message.longitude;
            altitude = message.altitude;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            Debug.Log("Latitude: " + latitude.ToString("F4") );
            Debug.Log("Longitude: " + longitude.ToString("F4"));
            Debug.Log("Altitude: " + altitude.ToString("F4"));
            isMessageReceived = false;
        }

    }
}

