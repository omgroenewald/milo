using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    
    public class InteractionObject : MonoBehaviour
    {
        private List<InteractionObject> requirements = new List<InteractionObject>();

        public string Ident => this.tag;
        public bool Owned;
        public bool CanUse;
        public bool CanPickUp;
        public bool KeepAfterUse;
        [HideInInspector]
        public bool Used;
        public bool CanUse2 => !RequiredForUse.Any(_ => !_.Owned);
        public bool ShouldRotate;
        public bool EndLevel;
        public Sprite Sprite;
        public List<InteractionObject> RequiredForUse = new List<InteractionObject>();
        public bool Use()
        {
            if (RequiredForUse.Any(_ => !_.Owned))
                return false;
            RequiredForUse.ForEach(_ => _.Used = _.Owned && _.KeepAfterUse);
            RequiredForUse.ForEach(_ => {

                _.Used = _.Owned && !_.KeepAfterUse;
                _.Owned = _.Owned && _.KeepAfterUse;
            });
            if (CanPickUp && !Owned)
            {
                Owned = true;
               //set render disabled to pick up item
                GetComponentInParent<Renderer>().enabled = false;
             }
            return true;
        }
        

    }
}
