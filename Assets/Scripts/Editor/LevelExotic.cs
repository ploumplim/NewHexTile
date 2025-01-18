using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = "LevelExoticCreator", menuName = "LevelExoticCreator", order = 0)]
    public class LevelExotic : ScriptableObject
    {
        public int gridX;
        public int gridY;
        public string levelName;
        public List<TileInfo> tiles = new List<TileInfo>();
    }

    [System.Serializable]
    public class TileInfo
    {
        public int x;
        public int y;
        public HexagonTile.TileStates tileState;
    }
}