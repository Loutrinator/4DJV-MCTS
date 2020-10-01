
using UnityEngine;

public class SimulatedPhysic 
{
   public static bool BoundsAreIntersecting(Bounds a, Bounds b)
   {
      return ((Mathf.Abs(a.min.x - b.min.x) * 2) <(a.size.x + b.size.x))
             && ((Mathf.Abs(a.min.y - b.min.y) * 2) <(a.size.y + b.size.y));
   }

   public static Vector3 NextPosition(Vector3 position, Vector3 velocity)
   {
        return  position + velocity * Time.deltaTime;
   }
}
