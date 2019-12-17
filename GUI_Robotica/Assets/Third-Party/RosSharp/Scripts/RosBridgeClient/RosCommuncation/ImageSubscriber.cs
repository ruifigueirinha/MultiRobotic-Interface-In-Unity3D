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
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class ImageSubscriber : Subscriber<Messages.Sensor.CompressedImage>
    {
        //public MeshRenderer meshRenderer;
        public RawImage raw_image;
        public RenderTexture renderTexture;
        public MeshRenderer meshRenderer;
        //public WebCamTexture camTexture;
        //public RenderTexture renderTexture;

        public Texture2D texture2D;
        private byte[] imageData;
        private bool isMessageReceived;

        private float width;
        private float height;

        private bool runOnce = false;

        private string tempTopic;

        protected override void Start()
        {



         //void OnEnable(){
		 	base.Start();
             //texture2D = new Texture2D(1, 1);
             //meshRenderer.material = new Material(Shader.Find("Standard"));
         }




        private void LateUpdate()
        {
            if (Topic != null && tempTopic != Topic)
            {
                tempTopic = Topic;
                base.Start();
                texture2D = new Texture2D(0, 0);
                //renderTexture = new RenderTexture(100, 100, 16, RenderTextureFormat.ARGB32);
                //renderTexture.Create();
            }

            if (isMessageReceived)
                ProcessMessage();
        
        
        }

        protected override void ReceiveMessage(Messages.Sensor.CompressedImage message)
        {
            imageData = message.data;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            //texture2D.LoadRawTextureData(imageData);
            texture2D.LoadImage(imageData);
            //texture2D.Compress(false);


            //Debug.Log("height " + texture2D.height);
            //Debug.Log("width " + texture2D.width);
            //width/height
             //



            //Debug.Log("Aspect Ratio " + gameObject.GetComponent<AspectRatioFitter>().aspectRatio);

            texture2D.Apply();
            raw_image.texture = texture2D;


            width = System.Convert.ToSingle(raw_image.texture.width);
            height = System.Convert.ToSingle(raw_image.texture.height);

            gameObject.GetComponent<AspectRatioFitter>().aspectRatio = width / height;
            //Graphics.Blit(texture2D, renderTexture);

            //meshRenderer.material.SetTexture("_MainTex", texture2D);
            isMessageReceived = false;
        }

    }
}

