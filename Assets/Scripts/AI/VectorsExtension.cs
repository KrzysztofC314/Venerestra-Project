using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public static class VectorsExtension
    {
        public static Vector3 WithAxis(this Vector3 vector, Axis axis, float value)
        {
            return new Vector3(
                x: axis == Axis.X ? value : vector.x,
                y: axis == Axis.Y ? value : vector.y,
                z: axis == Axis.Z ? value : vector.z
                );
        }
    }

    public enum Axis
    {
        X, Y, Z
    }
}
