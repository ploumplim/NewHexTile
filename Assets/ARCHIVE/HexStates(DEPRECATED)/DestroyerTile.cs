// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace HexStates
// {
//     
//     public class DestroyerTile : HexagonTile
//     {
//         public void Init()
//         {
//             HexagonTile thisTile = GetComponent<HexagonTile>();
//             int lifeTime = thisTile.destroyerLifeTime;
//             if (thisTile != null)
//             {
//                 thisTile.lifeTime = lifeTime;
//                 thisTile.isAlive = true;
//             }
//             
//         }
//         
//     }
//
// }