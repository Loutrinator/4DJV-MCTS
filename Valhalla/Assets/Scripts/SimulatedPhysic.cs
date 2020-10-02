using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PhysicTransform
{
    public Vector2 position;
    public Vector2 velocity;
}

public class SimulatedPhysic
{
    public static float gravity = 9.81f;

    public static float dampening
    {
        get
        {
            return GameManager.Instance.dampening;
        }
    }

    public static bool BoundsAreIntersecting(Bounds a, Bounds b)
    {
        return ((Mathf.Abs(a.min.x - b.min.x) * 2) <(a.size.x + b.size.x))
               && ((Mathf.Abs(a.min.y - b.min.y) * 2) <(a.size.y + b.size.y));
    }

    public static PhysicTransform Move(PhysicTransform tranform, Vector2 acceleration, float deltaTime)
    {
        Vector2 previousAcceleration = acceleration;
        tranform.velocity += acceleration;
        tranform.position += tranform.velocity * deltaTime;//(tranform.velocity + gravity*Vector2.down) * deltaTime;
        tranform.velocity = Vector2.Lerp(previousAcceleration, tranform.velocity, dampening * deltaTime);
        return tranform;
    }
}
