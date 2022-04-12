using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OMG.Assets.Scripts
{
    public class GameSave
    {
        public Level Level;
        public Player Player;
    }
    public class Level
    {
        public string Identity;
        public IEnumerable<string> OwnedItems;
        public IEnumerable<string> UsedItems;
    }
    public class Player
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationX;
        public float RotationY;
    }
    
}
