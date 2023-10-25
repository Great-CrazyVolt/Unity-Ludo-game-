using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObjectParent : PathPoint
{
    public PathPoint[] CommonPathPoint;
    public PathPoint[] RedPathPoint;
    public PathPoint[] BluePathPoint;
    public PathPoint[] YellowPathPoint;
    public PathPoint[] GreenPathPoint;
    public PathPoint[] BasePoints;
    public List<PathPoint> SafePoints;

    [Header("Scale and Positioning Difference")]
    public float[] scales;
    public float[] positionDifference;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
