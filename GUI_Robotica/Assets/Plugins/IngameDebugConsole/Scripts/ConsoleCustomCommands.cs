using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngameDebugConsole;

public class ConsoleCustomCommands : MonoBehaviour
{
    [ConsoleMethod("cube", "Creates a cube at specified position")]
    public static void CreateCubeAt(Vector3 position)
    {
        GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = position;
    }
}
