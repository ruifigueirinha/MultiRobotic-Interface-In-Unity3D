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
            case 0: // Open File
                if (pointCloudManager == null)
                {
                    Debug.Log("Instantiating PCL Manager");
                }
                pointCloudManager = Instantiate(pointCloudManagerPrefab);
                pointCloudManager.GetComponent<PointCloudManager>().OpenFileExplorer();
                break;
            case 1: // Options
                break;
            default:
                break;
        }
    }
}
