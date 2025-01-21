using System.Collections.Generic;
using UnityEngine;
using Editor;


namespace Editor
{
    [CreateAssetMenu(fileName = "LevelCreator", menuName = "LevelCreator", order = 0)]
    public class LevelExotic : ScriptableObject
    {
        public int gridX;
        public int gridY;
        public string levelName;
        public List<HexagonTile.TileStates> hexagonTileStates = new List<HexagonTile.TileStates>();
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