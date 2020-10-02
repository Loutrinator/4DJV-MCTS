using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedPhysic
{
    public static float gravity = 9.81f;
    public static float deltaTime = 0.01666666666f;
    
    public static bool BoundsAreIntersecting(Bounds a, Bounds b)
    {
        return ((Mathf.Abs(a.min.x - b.min.x) * 2) <(a.size.x + b.size.x))
               && ((Mathf.Abs(a.min.y - b.min.y) * 2) <(a.size.y + b.size.y));
    }

    public static Vector2 MoveTo(Vector2 position, Vector2 velocity)
    {
        return position + velocity * deltaTime;
    }
}
