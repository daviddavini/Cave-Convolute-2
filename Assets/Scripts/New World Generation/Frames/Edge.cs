using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sign {None, Negative, Positive};

public enum Orientation {None, Horizontal, Vertical};

public enum Edge {None, Left, Right, Bottom, Top};

public static class EdgeExtensions
{
    public static Edge Opposite(this Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return Edge.Right;
        case Edge.Right: return Edge.Left;
        case Edge.Top: return Edge.Bottom;
        case Edge.Bottom: return Edge.Top;
        default: return Edge.None;
        }
    }

    public static Sign GetSign(this Edge edge)
    {
        if (edge == Edge.Left || edge == Edge.Bottom) {return Sign.Negative;}
        else if (edge == Edge.Right || edge == Edge.Top) {return Sign.Positive;}
        return Sign.None;
    }

    public static Orientation GetOrientation(this Edge edge)
    {
        if (edge == Edge.Left || edge == Edge.Right) {return Orientation.Vertical;}
        else if (edge == Edge.Top || edge == Edge.Bottom) {return Orientation.Horizontal;}
        return Orientation.None;
    }

    public static bool IsHorizontal(this Edge edge)
    {
        return edge.GetOrientation() == Orientation.Horizontal;
    }

    public static bool IsVertical(this Edge edge)
    {
        return edge.GetOrientation() == Orientation.Vertical;
    }

    public static bool IsPositive(this Edge edge)
    {
        return edge.GetSign() == Sign.Positive;
    }

    public static bool IsNegative(this Edge edge)
    {
        return edge.GetSign() == Sign.Negative;
    }

    public static bool IsPerpendicularTo(this Edge edge1, Edge edge2)
    {
        return edge1.GetOrientation() != edge2.GetOrientation();
    }

    public static Edge[] Perpendiculars(this Edge edge)
    {
        if(edge.IsHorizontal()) {return new Edge[]{Edge.Left, Edge.Right};}
        else if(edge.IsVertical()) {return new Edge[]{Edge.Bottom, Edge.Top};}
        return new Edge[0];
    }

    public static string Abbreviation(this Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return "L";
        case Edge.Right: return "R";
        case Edge.Top: return "T";
        case Edge.Bottom: return "B";
        default: return "N";
        }
    }

    public static float ToAngle(this Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return 0;
        case Edge.Right: return (float)Math.PI;
        case Edge.Top: return (float)0.5f*Mathf.PI;
        case Edge.Bottom: return (float)1.5f*Mathf.PI;
        default: return 0;
        }
    }

    //inefficient, but works
    public static List<Edge> AllEdges()
    {
        List<Edge> allEdges = new List<Edge>((Edge[])Edge.GetValues(typeof(Edge)));
        allEdges.RemoveAt(0);
        return allEdges;
    }

    public static int ToRotationInt(this Edge edge)
    {
        switch(edge)
        {
        case Edge.Right: return 0;
        case Edge.Top: return 1;
        case Edge.Left: return 2;
        case Edge.Bottom: return 3;
        default: throw new ArgumentException("Edge.None has no rotation int");
        }
    }

    public static Edge RotatedRightTo(this Edge edge, Edge rotEdge)
    {
        switch ((edge.ToRotationInt() + rotEdge.ToRotationInt()) % 4)
        {
        case 0: return Edge.Right;
        case 1: return Edge.Top;
        case 2: return Edge.Left;
        case 3: return Edge.Bottom;
        default: return Edge.None;
        }
    }
}

public static class VectorEdgeExtensions
{
    public static Edge ToEdge(this Vector2 pos)
    {
        float angle = Vector2.SignedAngle(Vector2.right, pos);
        if (angle >= -45 && angle < 45)
            return Edge.Right;
        if (angle >= -135 && angle < -45)
            return Edge.Bottom;
        if (angle >= 45 && angle < 135)
            return Edge.Top;
        return Edge.Left;
    }
}

public static class ArrayEdgeExtensions
{
    public static Vector2Int EdgeDelta<T>(this T[,] array, Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return Vector2Int.up;
        case Edge.Right: return Vector2Int.up;
        case Edge.Top: return Vector2Int.right;
        case Edge.Bottom: return Vector2Int.right;
        default: return Vector2Int.zero;
        }
    }

    public static Vector2Int PerpEdgeDelta<T>(this T[,] array, Edge edge)
    {
        switch (edge)
        {
            case Edge.Left: return Vector2Int.right;
            case Edge.Right: return Vector2Int.left;
            case Edge.Top: return Vector2Int.down;
            case Edge.Bottom: return Vector2Int.up;
            default: return Vector2Int.zero;
        }
    }

    public static Vector2Int EdgeStart<T>(this T[,] array, Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return Vector2Int.zero;
        case Edge.Right: return new Vector2Int(array.GetLength(0)-1, 0);
        case Edge.Top: return new Vector2Int(0, array.GetLength(1)-1);
        case Edge.Bottom: return Vector2Int.zero;
        default: return Vector2Int.zero;
        }
    }
}

public static class OrientationExtensions
{
    public static Orientation Opposite(this Orientation ori)
    {
        if (ori == Orientation.Vertical) return Orientation.Horizontal;
        else if (ori == Orientation.Horizontal) return Orientation.Vertical;
        return Orientation.None;
    }

    public static Edge[] GetEdges(this Orientation ori)
    {
        switch(ori)
        {
        case Orientation.Horizontal: return new Edge[]{Edge.Top, Edge.Bottom};
        case Orientation.Vertical: return new Edge[]{Edge.Left, Edge.Right};
        default: return new Edge[0];
        }
    }
}

public static class SignExtensions
{
    public static Sign Opposite(this Sign sign)
    {
        if (sign == Sign.Positive) return Sign.Negative;
        else if (sign == Sign.Negative) return Sign.Positive;
        return Sign.None;
    }
}
