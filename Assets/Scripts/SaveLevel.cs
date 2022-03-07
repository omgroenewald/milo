using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class SaveLevel 
    {

        public string LevelIdentifier;
        public List<string> UsedItems;
        public List<string> OwnedItems;

        public SaveLevel()
        { }
        public SaveLevel(string levelText)
        {
            var splitText = levelText.Split(new string[] {"::"}, System.StringSplitOptions.None);
            if (splitText.Length != 3)
                return;
            LevelIdentifier = splitText[0];
            UsedItems = splitText[1].Split('#').ToList();
            OwnedItems = splitText[2].Split('#').ToList();

        }
        public override string ToString()
        {
            return ($"{LevelIdentifier}::{string.Join("#",OwnedItems)}::{string.Join("#",UsedItems)}");
        }
    }
}