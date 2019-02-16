using Assets.Scripts.Board;
using UnityEngine;

namespace AAGame
{
    public class GunControl
        : MonoBehaviour
    {
#pragma warning disable 649
        
        [SerializeField]
        private float horizontalCursorSpeed = 10;

        [SerializeField]
        private float verticalCursorSpeed = 10;

        [SerializeField]
        private float fireCooldown = 4;

        [SerializeField]
        private Transform cursor;

        [SerializeField]
        private Transform canon;

        [SerializeField]
        private Transform bulletPrefab;

        [SerializeField]
        private Transform bulletSpawn;

        [SerializeField]
        private TargetControl targetPlatform;

#pragma warning restore 649
        
        private Player player;
        private float cooldown;

        public Player Player
        {
            get => player;
            set
            {
                player = value;
                UpdateColors();
            }
        }

        public bool IsTargetDestroyed => targetPlatform.IsDestroyed;

        private void UpdateColors()
        {
            foreach(var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material.color = player.Colour;
            }
        }

        private void Update()
        {
            var x = Input.GetAxis(player.InputPrefix + "Horizontal") * horizontalCursorSpeed * Time.deltaTime;
            var z = Input.GetAxis(player.InputPrefix + "Vertical") * horizontalCursorSpeed * Time.deltaTime;

            float y = 0;
            if(Input.GetButton(player.InputPrefix + "Y"))
            {
                y = verticalCursorSpeed * Time.deltaTime;
            }
            else if(Input.GetButton(player.InputPrefix + "B"))
            {
                y = -verticalCursorSpeed * Time.deltaTime;
            }
            
            cursor.transform.Translate(x, y, z);
            canon.LookAt(cursor);

            if(Input.GetButton(player.InputPrefix + "A") && cooldown <= 0)
            {
                var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(canon.forward * 100, ForceMode.Impulse);
                cooldown = fireCooldown;
            }

            cooldown -= Time.deltaTime;
            cooldown = Mathf.Max(0, cooldown);
        }
    }
}
