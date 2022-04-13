using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
        public bool AllowZoom;
        public bool CanCraft;
        public InteractionObject CraftResult;
        public InteractionObject NeedToCraft;
        public bool UseGravity
        {
            get => gameObject.GetComponent<Rigidbody>()?.useGravity ?? false;
            set
            {
                var gravity = gameObject.GetComponent<Rigidbody>();
                if (gravity != null)
                {
                    gravity.useGravity = value;
                }
            }
        }
        public bool CanUse2 => !RequiredForUse.Any(_ => !_.Owned);
        public bool ShouldRotate;
        public bool EndLevel;
        public Sprite Sprite;
        public Animation animation;
        public List<InteractionObject> RequiredForUse;
        private Transform _dump;


        public void Use(Transform dump)
        {
            if (Used)
                return;
            _dump = dump;
            RequiredForUse?.ForEach(_ => _.Owned = _.Owned && _.KeepAfterUse);
            Used = true;
            if (CanPickUp && !Owned)
            {
                UseGravity = false;
                Owned = true;
                
               
                

                var render = GetComponentInParent<Renderer>();
                gameObject.transform.position = dump.position;

                //temp always try crafting on using.
                //Craft(NeedToCraft);
                Debug.Log($"after use {Owned}");
                //if (render != null)
                //{
                //    render.enabled = false;
                //}
            }
        }
        public InteractionObject Craft(InteractionObject craftFrom)
        {
            //can't craft this item
            if (!CanCraft)
            {
                Debug.Log("cant craft");
                return this;
            }
            //can't craft using this item
            if (NeedToCraft != craftFrom || !craftFrom.Owned)
            {
                Debug.Log("Can't craft with or don't own", NeedToCraft);
                return this;
            }
            Debug.Log($"Start craft own = {Owned}");
            Debug.Log("crafting");
            Used = true;
            craftFrom.Used = true;
            Owned = false;
            craftFrom.Owned = false;                    
            CraftResult.Owned = true;

            CraftResult.gameObject.transform.position = gameObject.transform.position;
            Hide(_dump);
            craftFrom.Hide(_dump);
            Debug.Log( $"End craft own = {Owned}");
            return CraftResult;
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
