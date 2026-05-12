using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] floorTiles;
    public GameObject[] objects;

    void Update()
    {
        if (Input.GetKey(InputConfig.MoveForward))
        {
            MoveElements();
        }
    }

    private void MoveElements()
    {
        MoveCollection(walls, GameConfig.EnvironmentWallThresholdZ);
        MoveCollection(floorTiles, GameConfig.EnvironmentFloorThresholdZ);
        MoveCollection(objects, GameConfig.EnvironmentFloorThresholdZ);
    }

    private void MoveCollection(GameObject[] elements, float thresholdZ)
    {
        foreach (GameObject element in elements)
        {
            MoveElement(element, thresholdZ);
        }
    }

    private void MoveElement(GameObject element, float thresholdZ)
    {
        element.transform.Translate(Vector3.back * GameConfig.EnvironmentMoveSpeed * Time.deltaTime);

        if (element.transform.position.z <= thresholdZ)
        {
            element.transform.position = new Vector3(element.transform.position.x, element.transform.position.y, GameConfig.EnvironmentResetPositionZ);
        }
    }
}
