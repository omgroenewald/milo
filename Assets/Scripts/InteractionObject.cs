using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class InteractionObject
    {
        public InteractionObject()
        {
            RequiredForUse = new List<InteractionObject>();
        }
        public string Id { get; set; }
        public bool Owned { get; set; }
        public bool CanUse { get; set; }
        public bool CanPickUp { get; set; }
        public bool KeepAfterUse { get; set; }
        public bool CanUse2 => !RequiredForUse.Any(_ => !_.Owned);
        public bool ShouldRotate { get; set; }
        public bool EndLevel { get; set; }
        public Sprite Sprite { get; set; }
        public List<InteractionObject> RequiredForUse {get;set;}
        public void Use()
        {
            RequiredForUse.ForEach(_ => _.Owned = _.Owned && _.KeepAfterUse);
            if (CanPickUp && !Owned)
                Owned = true;
        }
        

    }
}
