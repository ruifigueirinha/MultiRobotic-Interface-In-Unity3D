/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using TMPro;
using System;

namespace RosSharp.RosBridgeClient
{
    public class PoseStampedSubscriber : Subscriber<Messages.Geometry.PoseStamped>
    {
        public Transform PublishedTransform;
        public bool publish_transform; //enable do movimento do robot com base na pose
        private GameObject player_position;
        private GameObject attitude_gauge; //attitude foreground 1
        private GameObject attitude_gauge_roll;  //attitude foreground 1
        private GameObject compass;  //bussola
        private TMP_Text compass_text; //texto da bussola

        public TMP_Text roll_text;
        public TMP_Text pitch_text;
        private TMP_Text x_pos;
        private TMP_Text y_pos;
        private TMP_Text z_pos;

        public int pitch_offset;
        public int roll_offset;

        [HideInInspector]
        public float roll;
        [HideInInspector]
        public float pitch;
        [HideInInspector]
        public float yaw;

        [HideInInspector]
        public Vector3 position;
        private Quaternion rotation;
        [HideInInspector]
        public Vector3 position_raw;
        private Quaternion rotation_raw;

        [HideInInspector]
        public bool isMessageReceived;
        private float ball_rotation;


        protected override void Start()
        {
			base.Start();
            try
            {
                player_position = GameObject.FindGameObjectWithTag("Position").gameObject;
                x_pos = player_position.transform.GetChild(0).GetComponent<TMP_Text>();
                y_pos = player_position.transform.GetChild(1).GetComponent<TMP_Text>();
                z_pos = player_position.transform.GetChild(2).GetComponent<TMP_Text>();

                attitude_gauge = GameObject.FindGameObjectWithTag("Attitude_indicator").transform.GetChild(0).GetChild(0).gameObject; //tenta obter o gauge de attitude
                attitude_gauge_roll = GameObject.FindGameObjectWithTag("Attitude_indicator").transform.GetChild(0).GetChild(2).gameObject; //tenta obter o gauge de attitude
                compass = GameObject.FindGameObjectWithTag("Compass").transform.GetChild(0).gameObject; //Agulha da bussola
                compass_text = GameObject.FindGameObjectWithTag("Compass").transform.GetChild(2).GetComponent<TMP_Text>(); //Texto da bussola
            }
            catch
            {

            }
        }
		
        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(Messages.Geometry.PoseStamped message)
        {
            position = GetPosition(message).Ros2Unity(); //posiçao em eixos Unity
            rotation = GetRotation(message).Ros2Unity(); //posiçao em eixos Unity
            position_raw = GetPosition(message); //posiçao em eixos ROS
            rotation_raw = GetRotation(message); //rotaçao em eixos 

            Quat2Euler(rotation);
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            if (publish_transform)
            {
                PublishedTransform.position = position;
                PublishedTransform.rotation = rotation;
            }

            x_pos.text = "X: " + position.Unity2Ros().x.ToString("F3");
            y_pos.text = "Y: " + position.Unity2Ros().y.ToString("F3");
            z_pos.text = "Z: " + position.Unity2Ros().z.ToString("F3");

            //Debug.Log(rotation_raw.eulerAngles.z);
            if (attitude_gauge != null)
            {

                //Debug.Log(rotation_raw.eulerAngles.x);
                //Debug.Log(rotation_raw.eulerAngles.y);
                //Debug.Log(rotation_raw.eulerAngles.z);
                if (rotation_raw.eulerAngles.y > 180) // wrap around the angle
                    ball_rotation = 360 - rotation_raw.eulerAngles.y;

                attitude_gauge.transform.localPosition = new Vector3(0.0f, (ball_rotation + pitch_offset), 0.0f);
                attitude_gauge.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotation.eulerAngles.z + roll_offset);
                attitude_gauge_roll.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotation.eulerAngles.z + roll_offset);
                roll_text.text = "Roll: " + attitude_gauge.transform.localRotation.eulerAngles.z.ToString("F2");//ToString();
                pitch_text.text = "Pitch: " + (-attitude_gauge.transform.localPosition.y).ToString("F2");//ToString();
            }
            if (compass != null)
            {
                compass.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -rotation.eulerAngles.y);
                compass_text.text = Math.Round(rotation.eulerAngles.y).ToString();
            }
        }
        private Vector3 GetPosition(Messages.Geometry.PoseStamped message)
        {
            return new Vector3(
                message.pose.position.x,
                message.pose.position.y,
                message.pose.position.z);
        }

        private Quaternion GetRotation(Messages.Geometry.PoseStamped message)
        {
            return new Quaternion(
                message.pose.orientation.x,
                message.pose.orientation.y,
                message.pose.orientation.z,
                message.pose.orientation.w);
        }

        private void Quat2Euler(Quaternion rot)
        {
            
            roll = Mathf.Atan2(2 * rot.y * rot.w - 2 * rot.x * rot.z, 1 - 2 * rot.y * rot.y - 2 * rot.z * rot.z);
            pitch = Mathf.Atan2(2 * rot.x * rot.w - 2 * rot.y * rot.z, 1 - 2 * rot.x * rot.x - 2 * rot.z * rot.z);
            yaw = Mathf.Asin(2 * rot.x * rot.y + 2 * rot.z * rot.w);
        }
    }
}