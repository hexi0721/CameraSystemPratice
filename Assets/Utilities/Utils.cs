using UnityEngine;

public static class Utils
{
    public static Vector3 GetMouseWorldPos()
    {
        Vector3 mouserPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mouserPos;
    }

    public static Vector3 GetMouseWorldPosZeroZ()
    {
        Vector3 mouserPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouserPos.z = 0;
        return mouserPos;
    }
    
    public static float GetAngleFromVector(Vector3 dir)
    {
        float x = dir.x;
        float y = dir.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        return (angle < 0) ? angle + 360 : angle; 
    }
    

}