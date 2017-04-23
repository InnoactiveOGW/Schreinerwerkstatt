using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserstudyLvlCtrl : LevelControllerInterface
{
    Dictionary<string, Vector3> toolPositions = new Dictionary<string, Vector3>();
    Dictionary<string, Quaternion> toolRotations = new Dictionary<string, Quaternion>();

    void getToolsPosition()
    {
        GameObject[] tools = GameObject.FindGameObjectsWithTag("Tool");
        foreach (GameObject tool in tools)
        {
            toolPositions.Add(tool.name, tool.transform.position);
            toolRotations.Add(tool.name, tool.transform.rotation);
        }
    }

    public override void resetLevel()
    {
        foreach (KeyValuePair<string, Vector3> entry in toolPositions)
        {
            GameObject tool = GameObject.Find(entry.Key);
            tool.transform.position = entry.Value;
        }
        foreach (KeyValuePair<string, Quaternion> entry in toolRotations)
        {
            GameObject tool = GameObject.Find(entry.Key);
            tool.transform.rotation = entry.Value;
        }
    }

    void deleteAllWood()
    {
        GameObject[] woods = GameObject.FindGameObjectsWithTag("Wood");
        foreach(GameObject wood in woods)
        {
            Destroy(wood);
        }
    }
}
