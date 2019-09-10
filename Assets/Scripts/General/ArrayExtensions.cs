using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    public static void FillRect<T>(this T[,] array, T element, int i0, int i1, int j0, int j1)
    {
        for (int i = i0; i < i1; i++){
            for (int j = j0; j < j1; j++){
                array[i,j] = element;
            }
        }
    }

    public static T[,] WithBorder<T>(this T[,] array)
    {
        T[,] arrayWithBorder = new T[array.GetLength(0)+2, array.GetLength(1)+2];
        for (int i = 0; i < array.GetLength(0); i++){
            for (int j = 0; j < array.GetLength(1); j++){
                arrayWithBorder[i+1,j+1] = array[i,j];
            }
        }
        return arrayWithBorder;
    }

    public static int ConvolutionAt(this bool[,] array, bool[,] kernel, int i, int j)
    {
        int sum = 0;
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                if (array[i+x-1,j+y-1] && kernel[x,y]) {sum++;}
            }
        }
        return sum;
    }

    public static bool[,] Convolution(this bool[,] array, bool eye, bool[,] kernel, int cutoff = 1)
    {
        bool[,] arrayWithBorder = array.WithBorder();
        bool[,] convolution = new bool[array.GetLength(0), array.GetLength(1)];
        for (int i = 0; i < array.GetLength(0); i++){
            for (int j = 0; j < array.GetLength(1); j++){
                convolution[i,j] = array[i,j] == eye && arrayWithBorder.ConvolutionAt(kernel, i+1, j+1) >= cutoff;
            }
        }
        return convolution;
    }

    public static bool[,] Outline(this bool[,] array)
    {
        bool[,] kernel = new bool[,]{
          {true, true, true},
          {true, false, true},
          {true, true, true}
        };
        return array.Convolution(false, kernel);
    }

    public static bool[,] ThinOutline(this bool[,] array)
    {
        bool[,] kernel = new bool[,]{
          {false, true, false},
          {true, false, true},
          {false, true, false}
        };
        return array.Convolution(false, kernel);
    }

    public static bool[,] Interior(this bool[,] array)
    {
        bool[,] kernel = new bool[,]{
          {true, true, true},
          {true, false, true},
          {true, true, true}
        };
        return array.Convolution(true, kernel, 8);
    }

    public static void Add(this bool[,] array, bool[,] array2, float probability = 1, System.Random random = null)
    {
        for (int i = 0 ; i < array.GetLength(0); i++) {
            for (int j = 0 ; j < array.GetLength(1); j++) {
                array[i,j] = array[i,j] ||
                  (probability == 1 ? array2[i,j] : (random.NextDouble() < probability ? array2[i,j] : false));
            }
        }
    }

    public static int[,] ConvertToInt(this bool[,] array)
    {
        int[,] newArray = new int[array.GetLength(0), array.GetLength(1)];
        for (int i = 0 ; i < array.GetLength(0); i++) {
            for (int j = 0 ; j < array.GetLength(1); j++) {
                newArray[i,j] = array[i,j] ? 1 : 0;
            }
        }
        return newArray;
    }

    public static int Sum(this int[,] array)
    {
        int sum = 0;
        for (int i = 0 ; i < array.GetLength(0); i++) {
            for (int j = 0 ; j < array.GetLength(1); j++) {
                sum += array[i,j];
            }
        }
        return sum;
    }
}
