using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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

    public static GameState gameState;
    //Computes if the bound a collides with b bound
    public static bool BoundsAreIntersecting(Bounds a, Bounds b, out Vector2 diff)
    {
        /*
         *      ___
         *     |__|
         *  (minx, maxy)___max
         *             |__|
         *          min   (maxx, miny)
         *
         */
        float widthA = a.extents.x*2;
        float widthB = b.extents.x*2;
        float heightA = a.extents.x*2;
        float heightB = a.extents.x*2;

        bool isColliding = (a.center.x < b.center.x + widthB &&
                a.center.x + widthA > b.center.x &&
                a.center.y < b.center.y + heightB &&
                heightA + a.center.y > b.center.y);
        
        Vector2 aClosestCorner = GetClosestCornerToPoint(a, b.center);
        Vector2 bClosestCorner = GetClosestCornerToPoint(b, a.center);
        diff = bClosestCorner - aClosestCorner;
        
        return isColliding;
    }

    //Computes the closest corner of a bound to a given point in world space
    private static Vector2 GetClosestCornerToPoint(Bounds bound, Vector2 point)
    {
        Vector2 TL = new Vector2(bound.min.x, bound.max.y);
        Vector2 TR = new Vector2(bound.max.x, bound.max.y);
        Vector2 BR = new Vector2(bound.max.x, bound.min.y);
        Vector2 BL = new Vector2(bound.min.x, bound.min.y);
        Vector2[] aCorners = new[] {TL, TR, BL, BR};
        float min = int.MaxValue;
        Vector2 closestCorner = Vector2.zero;
        foreach (Vector2 corner in aCorners)
        {
            float dist = (corner - (Vector2) point).magnitude;
            if (dist < min)
            {
                min = dist;
                closestCorner = corner;
            }
        }
        return closestCorner;
    }
    //moves without checkingColisions
    public static PhysicTransform Move(PhysicTransform tranform, Vector2 acceleration, float deltaTime)
    {
        Vector2 previousAcceleration = acceleration;
        tranform.velocity += acceleration;
        tranform.position += (tranform.velocity + gravity*Vector2.down) * deltaTime;
        tranform.velocity = Vector2.Lerp(previousAcceleration, tranform.velocity, dampening * deltaTime);
        return tranform;
    }
    
    //moves and checks for collisions
    public static PhysicTransform Move(PhysicTransform transform, BoxCollider2D collider2D, Vector2 acceleration, float deltaTime)
    {   
        PhysicTransform transform2 = Move(transform,acceleration,deltaTime);
        
        //for each box collider in the level
        foreach (var levelCollider in gameState.levelColliders)
        {
            
            Vector2 diff;
            if(BoundsAreIntersecting(collider2D.bounds,levelCollider.bounds, out diff))
            {
                Debug.Log("OMG CA COLLIDE");
                transform.position += diff;
                break;
            }
        }
        return transform;
    }
}
