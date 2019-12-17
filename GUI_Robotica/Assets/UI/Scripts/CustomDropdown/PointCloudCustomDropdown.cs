using UnityEngine;

public class PointCloudCustomDropdown : CustomDropdown
{
    [SerializeField]
    private GameObject pointCloudManagerPrefab;
    private GameObject pointCloudManager;
    public override void ValueEvaluate(int index)
    {
        switch (index)
        {
            case 0:
                if (pointCloudManager != null)
                {
                    Debug.Log("Game object is instantiated");
                }
                else 
                {
                    Debug.Log("Game object is not instantiated, instantiating.....");
                    pointCloudManager = Instantiate(pointCloudManagerPrefab);
                    pointCloudManager.GetComponent<PointCloudManager>().OpenFileExplorer();
                }
                break;
            case 1:
                if (pointCloudManager != null)
                {
                    Debug.Log("Game object is instantiated");
                }
                else
                {
                    Debug.Log("Game object is not instantiated, instantiating.....");
                    pointCloudManager = Instantiate(pointCloudManagerPrefab);
                }
                break;
            case 2:
                if (pointCloudManager != null)
                {
                    Debug.Log("Game object is instantiated");
                }
                else
                {
                    Debug.Log("Game object is not instantiated, instantiating.....");
                    pointCloudManager = Instantiate(pointCloudManagerPrefab);
                }
                break;
            default:
                break;
        }
    }
}
