using System.Collections;
using TunnelTone.Events;
using TunnelTone.Gauge;
using TunnelTone.UI.PlayResult;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.Elements
{
    [RequireComponent(typeof(SphereCollider))]
    public class Tap : MonoBehaviour
    {
        public Vector2 position;
        public float time;
        private GameObject hitHint;
        private GameObject audioSource;
        private float offset;
        private bool isHit;
        private SphereCollider Collider => GetComponent<SphereCollider>();

        #region Load sprites

        private Sprite PerfectCritical => Resources.Load<Sprite>("Sprites/Perfect+");
        private Sprite Perfect => Resources.Load<Sprite>("Sprites/Perfect");
        private Sprite Great => Resources.Load<Sprite>("Sprites/Great");
        private Sprite Bad => Resources.Load<Sprite>("Sprites/Bad");
        
        #endregion
        
        private void Start()
        {
            gameObject.layer = 10;
            gameObject.tag = "Note";
            hitHint = new GameObject
            {
                transform =
                {
                    parent = GameObject.Find("Hit Zone").transform,
                    name = "Hit Hint",
                    localPosition = new Vector3()
                    {
                        x = position.x,
                        y = position.y,
                        z = 0
                    },
                    rotation = new Quaternion()
                    {
                      eulerAngles = new Vector3()
                      {
                          x = 0,
                          y = 0,
                          z = 45
                      }
                    },
                    localScale = new Vector3()
                    {
                        x = 0.15f,
                        y = 0.15f,
                        z = 1
                    }
                }
            };
            StartCoroutine(FadeColor(GetComponent<Image>()));
            var hitHintImage = hitHint.AddComponent<Image>();
            hitHintImage.sprite = Resources.Load<Sprite>("Sprites/HitHint");
            hitHintImage.color = new Color(1, 1, 1, 0);
            Collider.radius = 250;
            StartCoroutine(HitHint(hitHintImage));
            
            IntegrityGauge.OnSuddenDeath.AddListener(AssumeMiss);
            ChartEventReference.Instance.OnSongEnd.AddListener(OnSongEnd);
        }

        private void Update()
        {
            if (!(NoteRenderer.CurrentTime * 1000 > time + 120) || !NoteRenderer.isPlaying) return;
            ChartEventReference.Instance.OnNoteMiss.Trigger();
            Destroy();
        }

        private bool CheckZPosition(float target) => transform.position.z > target;

        private IEnumerator FadeColor(Graphic image)
        {
            yield return new WaitWhile(() => CheckZPosition(0));
            
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.1f);
            if (image.color.a <= 0)
            {
                yield break;
            }

            StartCoroutine(FadeColor(image));
        }

        private IEnumerator HitHint(Graphic image)
        {
            yield return new WaitUntil(() => NoteRenderer.CurrentTime * 1000 >= time - 500 && NoteRenderer.isPlaying);
            
            hitHint.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            LeanTween.value(hitHint, v3 => { hitHint.transform.localScale = v3; }, Vector3.one * 1.5f, Vector3.one * 1f, .5f);
            LeanTween.value(hitHint, c => { image.color = c; }, Color.clear, Color.white, .5f)
                .setOnComplete(() => Destroy(hitHint, 1.2f));
        }

        private IEnumerator ShowHitHint(Graphic image, GameObject hitHint)
        {
            yield return new WaitWhile(() => CheckZPosition(300));

            // Calculate new scale using a partial variation of the linear function y = ax + b, where 0.1 is the final scale
            var position1 = transform.position;
            var newScale = Mathf.Clamp(1 + 0.5f * position1.z / 300, 1, 1.5f);
            hitHint.transform.localScale = new Vector3(newScale, newScale, 1);
            hitHint.transform.position = new Vector3(position1.x, position1.y, 0);

            image.color = new Color()
            {
                r = 1,
                g = 1,
                b = 1,
                a = Mathf.Clamp(1 - position1.z / 300, 0, 1)
            };
            
            StartCoroutine(ShowHitHint(image, hitHint));
        }

        public void HitEffect(float hitOffset)
        {
            if (hitOffset >= 120) return;
            var sprite = hitOffset switch
            {
                <= 25 => PerfectCritical,
                <= 50 => Perfect,
                <= 100 => Great,
                <= 120 => Bad,
            };

            GameObject effect = (GameObject)Instantiate(Resources.Load("Prefabs/HitEffect"), hitHint.transform.position, Quaternion.identity, hitHint.transform.parent);
            effect.transform.localScale = Vector3.one * 1.25f;
            var component = effect.GetComponent<Image>();
            component.sprite = sprite;
            Destroy(effect, 0.34f);
            Destroy();
        }
        
        private void Destroy()
        {
            Destroy(hitHint);
            Destroy(gameObject);
        }
        
        internal float Hit(bool requireAbsoluteValue = true)
        {
            HitEffect(Mathf.Abs(offset = (float)(AudioSettings.dspTime - NoteRenderer.dspSongStartTime) * 1000 - time));
            ChartEventReference.Instance.OnNoteHit.Trigger(Mathf.Abs(offset));
            
            return requireAbsoluteValue ? Mathf.Abs(offset) : offset;
        }
        
        private void AssumeMiss(params object[] param)
        {
            ResultScreen.playResult.miss++;
            IntegrityGauge.OnSuddenDeath.RemoveListener(AssumeMiss);
        }

        private void OnSongEnd(params object[] param)
        {
            StopAllCoroutines();
            if (hitHint.gameObject != null) Destroy(hitHint.gameObject);
            enabled = false;
        }
        
        private void OnDestroy()
        {
            ChartEventReference.Instance.OnSongEnd.RemoveListener(OnSongEnd);
            IntegrityGauge.OnSuddenDeath.RemoveListener(AssumeMiss);
            Destroy(hitHint);
        }
    }
}