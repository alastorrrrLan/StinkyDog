using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    // changable petrol point, return the children number of the petrol prefab
    public int PointCount
    {
        get
        {
            return transform.childCount;
        }
    }
    
    // tell outside object which need "index"th point
    public Transform GetPoint(int index)
    {
        return transform.GetChild(index);
    }
}
