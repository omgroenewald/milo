//using System.Collections;
//using NUnit.Framework;
//using OMG.Assets.Scripts;
//using UnityEngine;
//using UnityEngine.TestTools;

//public class InteractionObjectTest
//{
//    private readonly Transform _dump;
//    public InteractionObjectTest()
//    { 
//        _dump = new GameObject().transform;
//    }
//    // A Test behaves as an ordinary method
//    [Test]
//    public void InteractionObjectTestSimplePasses()
//    {
//        // Use the Assert class to test conditions
//    }
//    [Test]
//    public void ShouldNotPickUpObjectIfCanPickupFalse()
//    {
//        var toTest = new InteractionObject
//        {
//            CanPickUp = true
//        };
//        toTest.Use(_dump);
//        Assert.True(toTest.Owned);
//    }

//    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
//    // `yield return null;` to skip a frame.
//    [UnityTest]
//    public IEnumerator InteractionObjectTestWithEnumeratorPasses()
//    {
//        // Use the Assert class to test conditions.
//        // Use yield to skip a frame.
//        yield return null;
//    }
//}
