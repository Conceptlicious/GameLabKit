using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public const int NORTH = 0, NORTH_EAST = 1, EAST = 2, SOUTH_EAST = 3,
        SOUTH = 4, SOUTH_WEST = 5, WEST = 6, NORTH_WEST = 7;

    private static System.Random rnd = new System.Random();
    public static void Shuffle<T>(this T[] array)
    {
        rnd = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            int k = rnd.Next(n);
            n--;
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public static void Shuffle<T>(T[,] array)
    {
        rnd = new System.Random();
        int lengthRow = array.GetLength(1);

        for (int i = array.Length - 1; i > 0; i--)
        {
            int i0 = i / lengthRow;
            int i1 = i % lengthRow;

            int j = rnd.Next(i + 1);
            int j0 = j / lengthRow;
            int j1 = j % lengthRow;

            T temp = array[i0, i1];
            array[i0, i1] = array[j0, j1];
            array[j0, j1] = temp;
        }
    }

    public static List<T> Shuffle<T>(List<T> list)
    {
        List<T> randomizedList = new List<T>();
        rnd = new System.Random();
        while (list.Count > 0)
        {
            int index = rnd.Next(0, list.Count); //pick a random item from the master list
            randomizedList.Add(list[index]); //place it at the end of the randomized list
            list.RemoveAt(index);
        }
        return randomizedList;
    }

    public static GameObject GetClickedGameObject()
    {
        // Builds a ray from camera point of view to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        if (Physics.Raycast(ray, out hit)) return hit.transform.gameObject;
        return null;
    }
}
