using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class FrameEntry
// {
//     //mandatory pre and post
//     public readonly Dictionary<Order, int> wallLengths;
//     public readonly int entryLength;
//
//     public FrameEntry(Dictionary<Order, int> wallLengths, int entryLength)
//     {
//         this.wallLengths = new Dictionary<Order, int>();
//         foreach(Order order in Order.GetValues(typeof(Order)))
//         {
//             this.wallLengths[order] = wallLengths[order];
//         }
//         this.entryLength = entryLength;
//     }
//
//     // inf [5] inf
//     public override string ToString()
//     {
//         string str = "";
//         int preWallLength;
//         int postWallLength;
//         str += wallLengths.TryGetValue(Order.Pre, out preWallLength) ? preWallLength.ToString() : "Inf";
//         str += " [" + entryLength + "] ";
//         str += wallLengths.TryGetValue(Order.Post, out postWallLength) ? postWallLength.ToString() : "Inf";
//         return str;
//     }
// }
