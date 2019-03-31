using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Bounds
{
    public float minX = 0f;
    public float maxX = 0f;
    public float minY = 0f;
    public float maxY = 0f;
    public float minZ = 0f;
    public float maxZ = 0f;

    public Vector3Bounds() { }
    public Vector3Bounds(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        this.minZ = minZ;
        this.maxZ = maxZ;
    }
}

public static class RandomExtension
{
    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    /// <summary>
    /// Return array of ints which is filed with index numbers in it
    /// </summary>
    /// <param name="size">size of array</param>
    /// <returns></returns>
    public static int[] RandomArray(int size)
    {
        int[] tab = new int[size];
        for (int i = 0; i < size; i++)
            tab[i] = i;
        return tab;
    }

    /// <summary>
    /// Returns array of Vector3s which are constrained to VectorBounds param
    /// </summary>
    /// <param name="size">size of array</param>
    /// <param name="bounds">bounds for random vectors</param>
    /// <returns></returns>
    public static Vector3[] RandomArray(int size, Vector3Bounds bounds)
    {
        Vector3[] tab = new Vector3[size];
        for (int i = 0; i < size; i++)
            tab[i] = new Vector3(Random.Range(bounds.minX, bounds.maxX), Random.Range(bounds.minY, bounds.maxY), Random.Range(bounds.minZ, bounds.maxZ));
        return tab;
    }
}
