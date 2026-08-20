using UnityEngine;
using System.Collections;
using OrbRaiders.Core;

namespace OrbRaiders.Combat
{
    public class TelegraphSystem : MonoBehaviour
    {
        public static TelegraphSystem Instance { get; private set; }

        [SerializeField] private GameObject circleTelegraphPrefab;
        [SerializeField] private GameObject lineTelegraphPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ShowCircleTelegraph(Vector3 center, float radius, float duration, System.Action onComplete)
        {
            StartCoroutine(CircleTelegraphRoutine(center, radius, duration, onComplete));
        }

        private IEnumerator CircleTelegraphRoutine(Vector3 center, float radius, float duration, System.Action onComplete)
        {
            GameObject dec = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            dec.name = "Telegraph_Circle";
            dec.transform.position = center + Vector3.up * 0.05f;
            dec.transform.localScale = new Vector3(radius * 2f, 0.01f, radius * 2f);

            var renderer = dec.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = new Color(1.0f, 0.1f, 0.1f, 0.35f);
            }

            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;

                if (renderer != null)
                {
                    float alpha = Mathf.Lerp(0.2f, 0.65f, progress) * (0.8f + Mathf.Sin(timer * 15f) * 0.2f);
                    renderer.material.color = new Color(1.0f, 0.1f, 0.1f, alpha);
                }
                yield return null;
            }

            Destroy(dec);
            onComplete?.Invoke();
        }

        public void ShowLineTelegraph(Vector3 start, Vector3 direction, float length, float width, float duration, System.Action onComplete)
        {
            StartCoroutine(LineTelegraphRoutine(start, direction, length, width, duration, onComplete));
        }

        private IEnumerator LineTelegraphRoutine(Vector3 start, Vector3 direction, float length, float width, float duration, System.Action onComplete)
        {
            GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
            line.name = "Telegraph_Line";
            Vector3 center = start + direction.normalized * (length * 0.5f);
            line.transform.position = center + Vector3.up * 0.05f;
            line.transform.rotation = Quaternion.LookRotation(direction);
            line.transform.localScale = new Vector3(width, 0.01f, length);

            var renderer = line.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = new Color(1.0f, 0.2f, 0.0f, 0.4f);
            }

            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            Destroy(line);
            onComplete?.Invoke();
        }
    }
}
