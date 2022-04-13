using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;

namespace OMG.Assets.Scripts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LevelManager : MonoBehaviour
    {
        // Start is called before the first frame update

        [JsonProperty]
        public string Identity { get => gameObject.name; }
        public List<InteractionObject> Objects;
        public UnityEvent HoverUseable;
        public UnityEvent IsUsing;
        public UnityEvent LevelDone;
        //public List<UnityEngine.UI.Image> InventorySlots = new List<UnityEngine.UI.Image>();
        public List<InventoryObject> InventorySlots;
        public Transform Dump;
        public UnityEvent<string> TextChange;
        public UnityEvent<GameObject> RotateObject;
        public Cams Player;
        public Camera MainCamera;
        public Camera RotationCamera;

        //should find a better way of doing this
        public Sprite EmptySlot;
        public static string LoadText;
        
       
        [JsonProperty]
        public List<InteractionObject> OwnedItems => Objects.Where(_ => _.Owned).ToList();
        [JsonProperty]
        public List<InteractionObject> UsedItems => Objects.Where(_ => _.Used).ToList();

        public void Start()
        {
            Objects = GameObject.FindObjectsOfType<InteractionObject>().ToList();
            Player = GameObject.FindObjectOfType<Cams>();
            
            Debug.Log($"interactional objects found {Objects.Count}");
            Debug.Log($"inventory objects found {InventorySlots.Count}");
            UpdateInventoryBag();
            if (!String.IsNullOrEmpty(LoadText))
            {
                Load(LoadText, false);
            }

        }

        /// <summary>
        /// Interaction event handler when raycast hit a collider
        /// </summary>
        /// <param name="hit">object hit by RayCast</param>
        /// <param name="click">is mouse clicked (should we interact)</param>
        public void Interact(RaycastHit hit, bool click)
        {
            //Check to see if we can interact with hit object
            HoverUseable?.Invoke();
            var ia = hit.collider.GetComponentInParent<InteractionObject>();
            if (ia is null) return;
            SetText(ia.LookText);
            if (ia != null && ia.CanUse2)
            {
                if (!click)
                {
                    //Can interact but click is not pressed
                    
                }
                else
                {
                    //Interact with object
                    InteractWithObject(ia);
                    IsUsing?.Invoke();
                    SetText(ia.UseText);
                }
            }
            if (click && !ia.CanUse2)
                SetText(ia.TryUseText);
            Debug.Log($"After interact raycast {ia.Owned}");
        }

        public void EnableRotate(GameObject obj)
        {
            Debug.Log(obj);
            RotateObject?.Invoke(obj);
            MainCamera.gameObject.SetActive(false);
            RotationCamera.gameObject.SetActive(true);
        }
        public void StopRotate()
        {
            MainCamera.gameObject.SetActive(true);
            RotationCamera.gameObject.SetActive(false);
        }

        /// <summary>
        /// Rotation event handler when raycast hit a collider
        /// </summary>
        /// <param name="hit">object hit by RayCast</param>
        public void Rotate(RaycastHit hit)
        {
            //Check to see if we can interact with hit object
            var ia = hit.collider.GetComponentInParent<InteractionObject>();
            if (ia != null && ia.ShouldRotate)
            {
                RotateObject?.Invoke(ia.gameObject);
            }
        }


        private void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;
            TextChange?.Invoke(text);
        }

        /// <summary>
        /// Interact with the item in question
        /// </summary>
        /// <param name="id">Id of the Interaction object</param>
        /// <param name="gObj">Underlying game object</param>
        private void InteractWithObject(InteractionObject o)
        {

            Debug.Log("in interact", this);
            //use item
            o.Use(Dump);
            o.Animate();
            Debug.Log($"after interact with {o.Owned}");
            Debug.Log($"useable items {OwnedItems.Count}", this);
            OwnedItems.ForEach(_ => print($"I picked up {_}"));
            if (o.EndLevel)
            {
                LevelDone?.Invoke();
                print("Winner winner chicken dinner");
            }
            UpdateInventoryBag();

        }
        private void UpdateInventoryBag()
        {
            //empty all slots
            InventorySlots.ForEach(slot => slot.Clear());

            OwnedItems                                                    //All interactive objects
                .Select((_, i) => new { index = i, iaObject =_ })     //get the index and the sprite 
                .ToList()
                .ForEach(_ => InventorySlots[_.index].InteractionObject = _.iaObject);    //set the correct inventory slot

        }

        public void Save(string fileName)
        {

            var save = new GameSave()
            {
                Level = new Level(){
                    Identity = SceneManager.GetActiveScene().name,
                    OwnedItems = this.OwnedItems.Select(_=>_.Identity),
                    UsedItems = this.UsedItems.Select(_=>_.Identity) },
                Player = new Scripts.Player(){
                    PositionX = Player.player.position.x,
                    PositionY = Player.player.position.y,
                    PositionZ = Player.player.position.z,
                    RotationX = Player.camaras.transform.rotation.x,
                    RotationY = Player.player.transform.rotation.y
                }
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(save));
           
        }
        public void Load(string fileName, bool loadLevel)
        {
            if (!File.Exists(fileName))
                return;
            string saveText = File.ReadAllText(fileName);
            var saveGame = JsonConvert.DeserializeObject<GameSave>(saveText);
            LoadText = fileName;
            if(loadLevel)
                SceneManager.LoadScene(saveGame.Level.Identity);

            saveGame.Level.OwnedItems.ToList().ForEach(owned => Objects.First(_ => _.Identity == owned).Hide(Dump).Owned = true);
            saveGame.Level.UsedItems.ToList().ForEach(used => Objects.First(_ => _.Identity == used).Hide(Dump).Used = true);
            UpdateInventoryBag();

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Save("blah.sav");
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                Load("blah.sav",true);
            }
        }
    
        void FixedUpdate()
        {
           
        }
    }
}