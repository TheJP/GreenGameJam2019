using UnityEngine;

namespace AAGame
{
    public class GunControl
        : MonoBehaviour
    {
        [SerializeField]
        private float horizontalCursorSpeed = 10;

        [SerializeField]
        private float verticalCursorSpeed = 10;

        [SerializeField]
        private Transform cursor;

        [SerializeField]
        private Transform canon;
        
        private void Update()
        {
            var x = Input.GetAxis("Horizontal") * horizontalCursorSpeed * Time.deltaTime;
            var z = Input.GetAxis("Vertical") * horizontalCursorSpeed * Time.deltaTime;

            float y = 0;
            if(Input.GetButton("Fire2"))
            {
                y = verticalCursorSpeed * Time.deltaTime;
            }
            else if(Input.GetButton("Fire3"))
            {
                y = -verticalCursorSpeed * Time.deltaTime;
            }
            
            cursor.transform.Translate(x, y, z);
            canon.LookAt(cursor);
        }
    }
}
