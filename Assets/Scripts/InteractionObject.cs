using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace OMG.Assets.Scripts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InteractionObject : MonoBehaviour
    {
        public InteractionObject()
        {
            RequiredForUse = new List<InteractionObject>();
        }
        public bool Owned;
        [JsonProperty]
        public string Identity { get => gameObject.name; }
        public bool Used;
        public bool Havedilog;
        public string LookText;
        public string UseText;
        public string TryUseText; 
        public bool CanPickUp;
        public bool KeepAfterUse;
        public bool CanUse2 => !RequiredForUse.Any(_ => !_.Owned);
        public bool ShouldRotate;
        public bool EndLevel;
        public Sprite Sprite;
        public Animation animation;
        public List<InteractionObject> RequiredForUse;
        
       
        public void Use(Transform dump)
        {
            RequiredForUse?.ForEach(_ => _.Owned = _.Owned && _.KeepAfterUse);
            Used = true;
            if (CanPickUp && !Owned)
            {
                Owned = true;
                    
                var render = GetComponentInParent<Renderer>();
                gameObject.transform.position = dump.position;
                //if (render != null)
                //{
                //    render.enabled = false;
                //}
            }
        }
        public InteractionObject Hide(Transform dump)
        {
            gameObject.transform.position = dump.position;
            return this;
        }
        public void Print(Text thetext)
        {
            if (Havedilog)
            {
               
                //thetext.GetComponent<UnityEngine.UI.Text>().text = Text.ToString();
                Console.WriteLine("doesthiswork");
            }
        }
        public void Rotate()
        { 

        }

        public void Animate()
        {
    
        }



    }
}
