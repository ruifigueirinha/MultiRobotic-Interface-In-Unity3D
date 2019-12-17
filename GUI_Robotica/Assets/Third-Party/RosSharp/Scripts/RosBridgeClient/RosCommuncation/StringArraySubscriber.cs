
//este script subscreve ao topico que publica todos os topicos a serem publicados de momento
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient.Messages.dds_unity
{
    [RequireComponent(typeof(RosConnector))]
    public class StringArraySubscriber : Subscriber<Messages.dds_unity.StringArray>
    {
        public string[] topics;
        public string[] types;
        public bool isMessageReceived;


        protected override void Start()
        {
			base.Start();
        }
        protected override void ReceiveMessage(Messages.dds_unity.StringArray message)
        {

            //for (int i = 0; i < topics.Length; i++)
            //{
            //    topics[i] = null;
            //    types[i] = null;
            //}
            topics = message.topics;
            types = message.types;

            //for (int j = 0; j < topics.Length; j++)
            //{
            //    Debug.Log("Topic: " + message.topics[j]);// + " Type: " + message.types[j]);
            //}
            //Debug.Log("topics" + topics[3]);
            //Debug.Log("message.topics" + message.topics[3]);

            //Debug.Log("types" + types[3]);
            //Debug.Log("message.types" + message.types[3]);
            //isMessageReceived = true;
        }
    }
}

