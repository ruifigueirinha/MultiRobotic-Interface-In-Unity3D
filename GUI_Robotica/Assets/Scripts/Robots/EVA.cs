using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;

public class EVA : RobotBase
{
    //[RequireComponent(typeof(RosConnector))]

    [System.Serializable]
    public class Thruster
    {
        public float rpm;
        public float voltage;
        public float current;
        public float thrustForce;
        public float percentage;
        public float temperature;
    }
    [System.Serializable]
    public class Sidescan
    {
        // Por implementar
    }
    [System.Serializable]
    public class SLS
    {
        public UnityEngine.Light LLight; // Luz esquerda
        public UnityEngine.Light RLight; // Luz direita
        public UnityEngine.Camera LCam;
        public UnityEngine.Camera RCam;
    }
    [System.Serializable]
    public class GPS
    {
        public float lat, lon, alt;
    }
    [System.Serializable]
    public class Battery
    {
        public float voltage;
        public float SoC;
        public float temperature;
    }
    public Thruster[] thrusters = new Thruster[12]; // 12 thrusters
    public Battery battery1 = new Battery();
    public Battery battery2 = new Battery();
    public Battery battery3 = new Battery();
    public SLS slsE = new SLS(); // SLS Esquerdo
    public SLS slsD = new SLS(); // SLS Direito
    public Sidescan sidescan = new Sidescan();
    public GPS gps = new GPS();

    public override void Awake() 
    {
        base.Awake();
    }
}
