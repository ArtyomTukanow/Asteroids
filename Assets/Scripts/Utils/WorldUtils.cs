using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class WorldUtils
    {
        /// <returns>Возвращает границы камеры на сцене</returns>
        public static Bounds GetWorldBounds(this Camera camera)
        {
            var worldStart = camera.ScreenToWorldPoint(Vector3.zero);
            var worldEnd = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight));
            return new Bounds(worldStart, worldEnd - worldStart);
        }

        public static Vector3 Add(this Vector3 vector3, float x = default, float y = default, float z = default) => 
            new Vector3(vector3.x + x, vector3.y + y, vector3.z + z);

        public static Vector3 Set(this Vector3 v, float? x = null, float? y = null, float? z = null) =>
            new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);

        public static float RandomSign(this float value) => value * Mathf.Sign(Random.Range(-1f, 1f));


        /// <summary>
        /// Вызывает универсальный метод <param name="staticMethod"/> с типом <param name="typesForGeneric"/>
        /// </summary>
        public static void InvokeMethodWithGenericTypes(Type classType, string staticMethod, IEnumerable<Type> typesForGeneric)
        {
            MethodInfo method = classType.GetMethod(staticMethod, BindingFlags.NonPublic | BindingFlags.Static);
            
            if(method == null)
                throw new Exception($"private static {classType.Name}.{staticMethod}() is not found!");

            foreach (var type in typesForGeneric)
            {
                try
                {
                    method.MakeGenericMethod(type).Invoke(null, null);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{classType.Name}.{staticMethod}<{type.Name}>() Can't Invoke!\n{e}");
                }
            }
        }

        public static Vector3 RandomPosition(this Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y), 
                Random.Range(bounds.min.z, bounds.max.z));
        }
    }
}