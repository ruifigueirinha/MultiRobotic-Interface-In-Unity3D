/*
Author: Linh Kaestner (linh4138@yahoo.de)

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RosSharp.RosBridgeClient
{
    public class DataPoint
    {

        public DataPoint() { }
        public Vector3 position;
        public Transform trans; 
        public float timeStamp;

        public DataPoint(float x, float y, Transform t)
        {

            position.x = x;
            position.y = 0;
            position.z = y;
            trans = t;
            timeStamp = Time.time;
        }
    }
}