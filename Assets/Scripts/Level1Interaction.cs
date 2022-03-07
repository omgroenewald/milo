using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Level1Interaction : MonoBehaviour
{
    // Start is called before the first frame update

    private InteractionObject key;
    private InteractionObject door;
    private InteractionObject drawer;
    private InteractionObject stick;
    public Dictionary<string,InteractionObject> Objects;
    public UnityEvent HoverUseable;
    public UnityEvent IsUsing;
    public UnityEvent LevelDone;
    public List<InteractionObject> io;
    public List<UnityEngine.UI.Image> InventorySlots = new List<UnityEngine.UI.Image>();

    //should find a better way of doing this
    public Sprite StickSprite ;
    public Sprite KeySprite;
    public Sprite EmptySlot;

    //mmmmmmm
    private int rotateCount = 0;
    private Transform rotateTransform;
    public float speed = 5f;
    public float RotAngleY =10f;
    public float AngleYStart =0;
    public float AngleXPotion = 10;
    public float AngleZpotion = 90;


    public List<string> OwnedItems => Objects.Values.Where(_ => _.Owned).Select(_ => _.Id).ToList();
    

    public void Start()
    {
        stick = new InteractionObject() { Id = "Stick", Owned = false, ShouldRotate = false, CanPickUp = true, Sprite = StickSprite, KeepAfterUse = true };
        key = new InteractionObject() { Id = "KeyMainDoor", Owned = false, ShouldRotate = false, CanPickUp = true, RequiredForUse = { stick }, Sprite = KeySprite };
        drawer = new InteractionObject() { Id = "DrawerA", Owned = false, ShouldRotate = false };
        door = new InteractionObject() { Id = "MainDoor", Owned = false, ShouldRotate = false, RequiredForUse = { key }, EndLevel = true };
        Objects = new Dictionary<string, InteractionObject>();
        AddObject(key);
        AddObject(drawer);
        AddObject(door);
        AddObject(stick);
        UpdateInventoryBag();

    }
    private void AddObject(InteractionObject myObject)
    {
        Objects.Add(myObject.Id, myObject);
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
        if (CanInteract(hit.collider.tag))
        {
            if (!click)
            {
                //Can interact but click is not pressed
                
                if (Objects[hit.collider.tag].ShouldRotate)
                {
                    rotateTransform = hit.collider.gameObject.transform;
                    rotateCount = 0;
                }
            }
            else
            {
                //Interact with object
                InteractWithObject(hit.collider.tag, hit.collider.gameObject);
                IsUsing?.Invoke();
            }
        }
    }

    /// <summary>
    /// Check to see if we can interact with item in RayCastHit
    /// </summary>
    /// <param name="Id">id of the object we wish to interact with</param>
    /// <returns></returns>
    bool CanInteract(string Id)
    {
        if (Objects.ContainsKey(Id))
        {
            //object in list
            return Objects[Id].CanUse2;
        }
        //object not in list
        return false;
    }
    /// <summary>
    /// Interact with the item in question
    /// </summary>
    /// <param name="id">Id of the Interaction object</param>
    /// <param name="gObj">Underlying game object</param>
    private void InteractWithObject(string id, GameObject gObj)
    {
        var item = Objects[id];
        //pick up (destroy) item if possible
        if (item.CanPickUp)
            Destroy(gObj);
        Objects[id].Use();
        OwnedItems.ForEach(_ => print($"I picked up {_}"));
        if (item.EndLevel)
        {
            LevelDone?.Invoke();
            print("Winner winner chicken dinner");
        }
        UpdateInventoryBag();

    }
    private void UpdateInventoryBag()
    {
        //empty all slots
        InventorySlots.ForEach(slot => slot.sprite = EmptySlot);
        
        Objects
            .Values                                                     //All interactive objects
            .Where(_ => _.Owned)                                        //that we own
            .Select((_, i) => new { index = i, sprite = _.Sprite })     //get the index and the sprite 
            .ToList()                                                   
            .ForEach(_=> InventorySlots[_.index].sprite = _.sprite);    //set the correct inventory slot
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (rotateTransform!= null && rotateCount++ < 10)
        {
            float rY = Mathf.SmoothStep(AngleYStart, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
            rotateTransform.rotation = Quaternion.Euler(AngleXPotion, rY, AngleZpotion);
        }
    }
}
