/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

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

// Adjustments to new Publication Timing and Execution Framework 
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using UnityEditor.VersionControl;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class PublishImagePostProcessing : Publisher<Messages.Sensor.Image>
    {

        public Camera ImageCamera;
        public string FrameId = "Camera";
        public int resolutionWidth = 640;
        public int resolutionHeight = 480;
        [Range(0, 100)]
        public int qualityLevel = 50;

        private Messages.Sensor.Image message;
        private Texture2D texture2D;

        public RenderTexture renderTexture;
        private Rect rect;

        public Material material;

        protected override void Start()
        {
            base.Start();
            InitializeGameObject();
            InitializeMessage();
            Camera.onPostRender += UpdateImage;
        }

        private void UpdateImage(Camera _camera)
        {
            if (texture2D != null && _camera == this.ImageCamera)
                UpdateMessage();
        }

        private void InitializeGameObject()
        {
            texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
            rect = new Rect(0, 0, resolutionWidth, resolutionHeight);
            //ImageCamera.targetTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        }

        private void InitializeMessage()
        {
            message = new Messages.Sensor.Image();
            message.header.frame_id = FrameId;
            message.encoding = "rgb8";
            message.is_bigendian = 0;
        }

        private void UpdateMessage()
        {
            message.header.Update();
                 
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);   
            texture2D.GetPixels(0,0, renderTexture.width, renderTexture.height); 
            message.height = renderTexture.height;       
            message.width = renderTexture.width;       
        

            Graphics.Blit(texture2D, material, -1);
            texture2D.Apply();
            //byte[] bytes;
            //bytes = texture2D.EncodeToPNG();
            //message.data = bytes;

            message.data = texture2D.GetRawTextureData();
            // message.data = texture2D.EncodeToPNG();
            message.step = sizeof(byte) * 3 * message.width; // 1 * 3 * 480    image.cols * number_of_channels * sizeof(datatype_used)


            Publish(message);
        }

    }
}
