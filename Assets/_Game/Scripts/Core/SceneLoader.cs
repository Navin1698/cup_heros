using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrbRaiders.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        private readonly string[] gameplayTips = new string[]
        {
            "Try combining Frost Core and Chain Energy to freeze entire hordes!",
            "Ember Golem's Ground Slam attack shows a red warning circle. Move out in time!",
            "Orb Split increases your projectile count and spread angle dramatically.",
            "Upgrade hero base attributes with Gold in the Main Menu between runs.",
            "Flame Core applies stackable burn damage over time to high-HP targets.",
            "Trigger Nova's CORE BURST ultimate when surrounded by dense enemy waves."
        };

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName, System.Action onComplete = null)
        {
            StartCoroutine(LoadSceneRoutine(sceneName, onComplete));
        }

        private IEnumerator LoadSceneRoutine(string sceneName, System.Action onComplete)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            op.allowSceneActivation = true;

            yield return new WaitUntil(() => op.isDone);
            onComplete?.Invoke();
        }

        public string GetRandomTip()
        {
            return gameplayTips[Random.Range(0, gameplayTips.Length)];
        }
    }
}
