// //Rui Figueirinha


using UnityEngine;
using RosSharp.RosBridgeClient;
using rosapi = RosSharp.RosBridgeClient.Services.RosApi;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class ServiceCaller: MonoBehaviour 
    {	

		public string ServiceName;

        protected virtual void Start()
        {
            //GetComponent<RosConnector>().RosSocket.CallService<Tin, Tout>(ServiceName, GetTopics.ServiceCallHandler, ServiceArgs);//ServiceCallHandler, ServiceArgs);
            GetComponent<RosConnector>().RosSocket.CallService<rosapi.TopicsRequest, rosapi.TopicsResponse>("rosapi/topics", ServiceCallHandler, new rosapi.TopicsRequest());
            //GetComponent<RosConnector>().RosSocket.CallService<rosapi.GetParamRequest, rosapi.GetParamResponse>("/rosapi/get_param", ServiceCallHandler, new rosapi.GetParamRequest("/rosdistro", "default"));
        }

        //public bool ServiceCallHandler(rosapi.TopicsResponse  response);
        private static void ServiceCallHandler(rosapi.TopicsResponse message)
        {
            foreach(string value in message.topics)
                Debug.Log("ROS distro: " + message.topics);


            //Debug.Log("ROS distro: " + message.value);
        }


    }
}