using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Level1Interaction : MonoBehaviour
{
    // Start is called before the first frame update

    //public Dictionary<string,InteractionObject> Objects;
    public UnityEvent HoverUseable;
    public UnityEvent IsUsing;
    public UnityEvent LevelDone;
    public List<InteractionObject> Objects = new List<InteractionObject>();
    public List<UnityEngine.UI.Image> InventorySlots = new List<UnityEngine.UI.Image>();

    //should find a better way of doing this
    public Sprite StickSprite ;
    public Sprite KeySprite;
    public Sprite EmptySlot;

    public string LevelIdentifier;

    //mmmmmmm
    private int rotateCount = 0;
    private Transform rotateTransform;
    public float speed = 5f;
    public float RotAngleY =10f;
    public float AngleYStart =0;
    public float AngleXPotion = 10;
    public float AngleZpotion = 90;


    public List<string> OwnedItems => Objects.Where(_ => _.Owned).Select(_ => _.Ident).ToList();
    public List<string> UsedItems => Objects.Where(_ => _.Used).Select(_ => _.Ident).ToList();


    public void Start()
    {
        UpdateInventoryBag();

    }
    /// <summary>
    /// Interaction event handler when raycast hit a collider
    /// </summary>
    /// <param name="hit">object hit by RayCast</param>
    /// <param name="click">is mouse clicked (should we interact)</param>
    public void Interact(RaycastHit hit, bool click)
    {
        //Check to see if we can interact with hit object
        
        var ia = hit.collider.GetComponentInParent<InteractionObject>();
        if (ia != null)
        {
            HoverUseable?.Invoke();
            if (ia.CanUse2)
            {
                if (!click)
                {
                    //Can interact but click is not pressed

                    if (ia.ShouldRotate)
                    {
                        rotateTransform = hit.collider.gameObject.transform;
                        rotateCount = 0;
                    }
                }
                else
                {

                    IsUsing?.Invoke();
                    //Interact with object
                    ia.Use();
                    if (ia.EndLevel)
                        LevelDone?.Invoke();
                    UpdateInventoryBag();
                }
            }
        }
        
    }

    private void UpdateInventoryBag()
    {
        //empty all slots
        InventorySlots.ForEach(slot => slot.sprite = EmptySlot);
        
        Objects
            .Where(_ => _.Owned)                                        //objects that we own
            .Select((_, i) => new { index = i, sprite = _.Sprite })     //get the index and the sprite 
            .ToList()                                                   
            .ForEach(_=> InventorySlots[_.index].sprite = _.sprite);    //set the correct inventory slot
        
    }

    public void Save()
    {
       var save = new SaveLevel() { LevelIdentifier = LevelIdentifier, OwnedItems = OwnedItems, UsedItems = UsedItems };
        print(save.ToString());
    }
    public void Load(string loadText)
    {
        var save = new SaveLevel(loadText);
        
            Objects.Where(_ => save.UsedItems.Contains(_.Ident)).ToList().ForEach(SetUsed);

        Objects.Where(_ => save.OwnedItems.Contains(_.Ident)).ToList().ForEach(SetOwned);
    }

    private void SetUsed(InteractionObject obj)
    {
        obj.Used = true;
        obj.Owned = false;
        obj.GetComponentInParent<Renderer>().enabled = false;
    }
    private void SetOwned(InteractionObject obj)
    {
        obj.Owned = true;
        obj.GetComponentInParent<Renderer>().enabled = false;
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
