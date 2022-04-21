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
        /// <summary>
        /// Should always use my location for new crafted object.
        /// </summary>
        public bool PrimaryObject;
        public InteractionObject CraftResult;
        public InteractionObject NeedToCraft;


        private void OnCollisionEnter(Collision collision)
        {
            var collisionObject = collision.gameObject.GetComponent<InteractionObject>();

            if (collisionObject != null && !Owned)
            {
                Craft(collisionObject);
            }
        }

        public void Awake()
        {
            _dump = GameObject.FindGameObjectsWithTag("Dump").First().transform;
        }

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
                if (gravity = null)
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
        public Transform Hands;

        public void Drop()
        {
            Owned = false;
            Used = false;
            UseGravity = true;
        }
        public void Use()
        {
            if (Used)
                return;
            RequiredForUse?.ForEach(_ => _.Owned = _.Owned && _.KeepAfterUse);
            Used = true;
            if (CanPickUp && !Owned)
            {
                UseGravity = false;
                Owned = true;

                Hide();
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
            if (NeedToCraft != craftFrom )
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
            //CraftResult.Owned = true;

            CraftResult.gameObject.transform.position = ChooseCraftPosition(craftFrom);
            CraftResult.UseGravity = true;
            Debug.Log($"old object position {gameObject.transform.position}", gameObject);
            Debug.Log($"craft position {CraftResult.gameObject.transform.position}", CraftResult);
            Debug.Log($"dump position {_dump.position}", _dump);

            Hide();
            craftFrom.Hide();
            return CraftResult;
        }

        private Vector3 ChooseCraftPosition(InteractionObject craftFrom)
        {
            if (craftFrom.PrimaryObject) 
                return CraftResult.transform.position;  //Other object is primary don't use my location
            return (gameObject.transform.position == _dump.position) 
                ? CraftResult.transform.position   //I'm in dump don't put Craft object here.
                : gameObject.transform.position;  //Set Craft object location to my location

        }

        public InteractionObject Hide()
        {
            gameObject.transform.position = _dump.position;
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

        public void Throw(Transform Hands)
        {
            Owned = false;
            Used = false;
            UseGravity = true;
        }

    }
}
