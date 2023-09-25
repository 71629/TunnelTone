using System.Collections;
using TunnelTone.Events;
using UnityEngine;
using UnityEngine.UI;

namespace TunnelTone.Elements
{
    public class Tap : MonoBehaviour
    {
        public Vector2 position;
        public float time;
        private GameObject hitHint;
        private GameObject audioSource;

        private void Start()
        {
            hitHint = new GameObject()
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
            StartCoroutine(ShowHitHint(hitHintImage, hitHint));
        }

        private bool CheckZPosition(float target) => transform.position.z > target;

        private IEnumerator FadeColor(Image image)
        {
            yield return new WaitWhile(() => CheckZPosition(0));
            
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.1f);
            if (image.color.a <= 0)
            {
                Destroy(0);
                yield break;
            }

            StartCoroutine(FadeColor(image));
        }

        private IEnumerator ShowHitHint(Image image, GameObject hitHint)
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
        public void Hit()
        {
            var offset = Mathf.Abs(NoteRenderer.dspSongStartTime - time);
            
            if (offset <= 50)
            {
                Debug.Log("Perfect");
                GetComponent<Image>().color = Color.cyan;
                Destroy(1);
            }
            if (offset <= 100)
            {
                Debug.Log("Off");
                GetComponent<Image>().color = Color.green;
                Destroy(1);
            }
            Debug.Log("Way off");
            Destroy(1);
        }
        
        private void Destroy(float delay)
        {
            Destroy(hitHint);
            Destroy(this.gameObject, delay);
        }
    }
    
    
}