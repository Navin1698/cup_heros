using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Progression
{
    public class XPOrb : MonoBehaviour
    {
        [SerializeField] private float attractionRadius = 5.0f;
        [SerializeField] private float moveSpeed = 12.0f;

        private int xpValue = 15;
        private Transform playerTransform;
        private bool isAttracting = false;

        public static void SpawnOrb(Vector3 position, int xpValue)
        {
            GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.name = "XP_Orb";
            orb.transform.position = position + Vector3.up * 0.4f;
            orb.transform.localScale = Vector3.one * 0.45f;

            var renderer = orb.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = new Color(0.0f, 0.95f, 1.0f); // Turquoise glowing XP orb
            }

            var col = orb.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;

            var comp = orb.AddComponent<XPOrb>();
            comp.xpValue = xpValue;
        }

        private void Start()
        {
            if (Player.PlayerController.Instance != null)
            {
                playerTransform = Player.PlayerController.Instance.transform;
            }
        }

        private void Update()
        {
            if (playerTransform == null && Player.PlayerController.Instance != null)
            {
                playerTransform = Player.PlayerController.Instance.transform;
            }

            if (playerTransform == null) return;

            float dist = Vector3.Distance(transform.position, playerTransform.position);

            if (dist <= attractionRadius)
            {
                isAttracting = true;
            }

            if (isAttracting)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position + Vector3.up * 0.8f, moveSpeed * Time.deltaTime);

                if (dist < 0.6f)
                {
                    Collect();
                }
            }
        }

        private void Collect()
        {
            if (Player.PlayerController.Instance != null && Player.PlayerController.Instance.Experience != null)
            {
                Player.PlayerController.Instance.Experience.AddXP(xpValue);
            }

            if (Services.HapticManager.Instance != null)
            {
                Services.HapticManager.Instance.TriggerLight();
            }

            Destroy(gameObject);
        }
    }
}
