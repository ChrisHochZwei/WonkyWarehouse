using UnityEngine;

namespace _Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        [SerializeField] private Camera mainCamera = null;
        [SerializeField] private Camera forkCamera = null;
        [SerializeField] private Camera sideCamera = null;
        [SerializeField] private float distance = 3.0f;
        [SerializeField] private float height = 1.0f;
        [SerializeField] private float damping = 5.0f;
        [SerializeField] private bool smoothRotation = true;
        [SerializeField] private float rotationDamping = 10.0f;

        [SerializeField]
        private Vector3 targetLookAtOffset; // allows offsetting of camera lookAt, very useful for low bumper heights

        [SerializeField] private float bumperDistanceCheck = 2.5f; // length of bumper ray
        [SerializeField] private float bumperCameraHeight = 1.0f; // adjust camera height while bumping
        [SerializeField] private Vector3 bumperRayOffset; // allows offset of the bumper ray from target origin

        [SerializeField] private Camera currentCamera;

        private float currentLerpTime;

        /// <Summary>
        /// If the target moves, the camera should child the target to allow for smoother movement. DR
        /// </Summary>
        private void Awake()
        {
            currentCamera = mainCamera;
            mainCamera.enabled = currentCamera == mainCamera;
            forkCamera.enabled = currentCamera == forkCamera;
            sideCamera.enabled = currentCamera == sideCamera;
        }

        private void FixedUpdate()
        {
            currentLerpTime = 0;
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (currentCamera == mainCamera)
                {
                    currentCamera = forkCamera;
                } else if (currentCamera == forkCamera)
                {
                    currentCamera = sideCamera;
                } else if (currentCamera == sideCamera)
                {
                    currentCamera = mainCamera;
                }
                
                mainCamera.enabled = currentCamera == mainCamera;
                forkCamera.enabled = currentCamera == forkCamera;
                sideCamera.enabled = currentCamera == sideCamera;
            }

            if (currentCamera == mainCamera)
            {
                Vector3 wantedPosition = target.TransformPoint(0, height, -distance);

                // check to see if there is anything behind the target
                RaycastHit hit;
                Vector3 back = target.transform.TransformDirection(-1 * Vector3.forward);

                // cast the bumper ray out from rear and check to see if there is anything behind
                if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck)
                    && hit.transform != target) // ignore ray-casts that hit the user. DR
                {
                    // clamp wanted position to hit position
                    wantedPosition.x = hit.point.x;
                    wantedPosition.z = hit.point.z;
                    wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y,
                        Time.deltaTime * damping);
                }

                transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

                Vector3 lookPosition = target.TransformPoint(targetLookAtOffset);

                if (smoothRotation)
                {
                    Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation,
                        Time.deltaTime * rotationDamping);
                }
                else
                    transform.rotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
            }
            else if (currentCamera == forkCamera)
            {
                transform.position = forkCamera.transform.position;
                transform.rotation = forkCamera.transform.rotation;
            } else if (currentCamera == sideCamera)
            {
                transform.position = sideCamera.transform.position;
                transform.rotation = forkCamera.transform.rotation;
            }
        }
    }
}