using UnityEngine;
using UnityEngine.Events;

namespace OMG.Assets.Scripts
{
    class RotationManager : MonoBehaviour
    {
        public Transform RotationLocation;
        public Transform DumpLocation;
        public Camera RotationCamera;
        private GameObject _objectToRotate;
        public float Sensitivity = .25f;
        public UnityEvent StopRotation; 

        public bool InRotation = false;
        private Quaternion _rotateBy;
        private bool _newDeltaObtained = false;


        public void Rotate(GameObject objectToRotate)
        {
            //remove previous rotation objects
            Clear();
            //move object to rotation location

            objectToRotate.transform.position = RotationLocation.position;
            Debug.Log($"{objectToRotate} in position");
            _objectToRotate = objectToRotate;
            //_objectToRotate.transform.localScale = MiddleR.localScale;
            //_objectToRotate.transform.parent = MiddleR;

            //set camera
            InRotation = true;
            // Check the number of monitors connected.
            //if (Display.displays.Length > 1)
            //{
            //    // Activate the display 1 (second monitor connected to the system).
            //    Display.displays[2].Activate();
            //}

        }

        public void Clear()
        {
            //check to see if there is a previous object in rotation location
            if (_objectToRotate != null && _objectToRotate.transform.position == RotationLocation.position)
            {
                _objectToRotate.transform.position = DumpLocation.position;
                _objectToRotate = null;
            }
        }
        public void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                StopRotation?.Invoke();
            if (_objectToRotate is null || !Input.GetMouseButton(0))
                return;
            Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public void Update()
        {


            if (_newDeltaObtained)
            {
                _objectToRotate.transform.localRotation *= _rotateBy;

                _newDeltaObtained = false;
            }


        }

        public void Rotate(float rotateLeftRight, float rotateUpDown)
        {
            if (!InRotation)
                return; 
            //useUpdate = false;

            //Unsure of how much below code changes outcome.

            //Gets the world vector space for cameras up vector 
            Vector3 relativeUp = RotationCamera.transform.TransformDirection(Vector3.down);
            //Gets world vector for space cameras right vector
            Vector3 relativeRight = RotationCamera.transform.TransformDirection(Vector3.left);

            //Turns relativeUp vector from world to objects local space
            Vector3 objectRelativeUp = _objectToRotate.transform.InverseTransformDirection(relativeUp);
            //Turns relativeRight vector from world to object local space
            Vector3 objectRelaviveRight = _objectToRotate.transform.InverseTransformDirection(relativeRight);

            _rotateBy = Quaternion.AngleAxis(rotateLeftRight / Sensitivity, objectRelativeUp)
                * Quaternion.AngleAxis(-rotateUpDown / Sensitivity, objectRelaviveRight);

            _newDeltaObtained = true;

        }

    }
}
