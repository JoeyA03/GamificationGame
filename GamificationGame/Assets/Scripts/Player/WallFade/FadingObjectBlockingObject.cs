using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingObjectBlockingObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private float fadedAlpha = 0.33f;
    [SerializeField]
    private FadeMode FadingMode;

    [SerializeField]
    private float checkPerSecond = 10;
    [SerializeField]
    private int fadeFPS = 30;
    [SerializeField]
    private float fadeSpeed = 1;

    [Header("READ ONLY")]
    [SerializeField]
    private List<FadingObject> objectsBlockingView = new List<FadingObject>();
    private List<int> indexToClear = new List<int>();
    private Dictionary<FadingObject, Coroutine> runningCoroutines = new Dictionary<FadingObject, Coroutine>();

    private RaycastHit[] Hits = new RaycastHit[10];

    private void Start()
    {
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / checkPerSecond);

        while (true)
        {
            int hits = (Physics.RaycastNonAlloc(camera.transform.position, (Player.transform.position - camera.transform.position).normalized, Hits, Vector3.Distance(camera.transform.position, Player.transform.position), layerMask));
            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(Hits[i]);

                    if (fadingObject != null && !objectsBlockingView.Contains(fadingObject))
                    {
                        if (runningCoroutines.ContainsKey(fadingObject))
                        {
                            if (runningCoroutines[fadingObject] != null) // may be null if it already finished
                            {
                                StopCoroutine(runningCoroutines[fadingObject]);
                            }
                        }

                        runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                        objectsBlockingView.Add(fadingObject);

                    }

                }
            }

            FadeObjectsNoLongerBeingHit();

            ClearHits();

            yield return wait;
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        for (int i = 0; i < objectsBlockingView.Count; i++)
        {
            bool objectIsBeingHit = false;
            for (int j = 0; j < Hits.Length; j++)
            {
                FadingObject fadingObj = GetFadingObjectFromHit(Hits[j]);
                if (fadingObj != null && fadingObj == objectsBlockingView[i]) 
                {
                    objectIsBeingHit = true;
                    break;
                }
            }

            if (!objectIsBeingHit) 
            {
                if (runningCoroutines.ContainsKey(objectsBlockingView[i])) 
                {
                    if (runningCoroutines[objectsBlockingView[i]] != null)
                    {
                        StopCoroutine(runningCoroutines[objectsBlockingView[i]]);
                    }
                    runningCoroutines.Remove(objectsBlockingView[i]);
                }

                runningCoroutines.Add(objectsBlockingView[i], StartCoroutine(FadeObjectIn(objectsBlockingView[i])));
                objectsBlockingView.RemoveAt(i);
            }
        }
    }

    private IEnumerator FadeObjectOut(FadingObject fadingObj) 
    {
        float waitTime = 1f / fadeFPS;
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        for (int i = 0; i < fadingObj.Materials.Count; i++)     // CHANGE ALL OF THE MATERIALS FROM OPAQUE TO TRANSPARENT
        {
            fadingObj.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);  // affects both Transparrent and Fade options of unity mats
            fadingObj.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);  // affects both Transparrent and Fade options of unity mats
            fadingObj.Materials[i].SetInt("_ZWrite", 0);  // disable Z Writing

            if (FadingMode == FadeMode.Fade)
            {
                fadingObj.Materials[i].EnableKeyword("_ALPHABLEND_ON");
            }
            else 
            {
                fadingObj.Materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            fadingObj.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        if (fadingObj.Materials[0].HasProperty("_Color")) // fade material property
        {
            while (fadingObj.Materials[0].color.a > fadedAlpha) 
            {
                for (int i = 0; i < fadingObj.Materials.Count; i++)
                {
                    if (fadingObj.Materials[i].HasProperty("_Color")) 
                    {
                        fadingObj.Materials[i].color = new Color(
                            fadingObj.Materials[i].color.r,
                            fadingObj.Materials[i].color.g,
                            fadingObj.Materials[i].color.b,
                            Mathf.Lerp(fadingObj.InitialAlpha, fadedAlpha, waitTime * ticks * fadeSpeed)
                            );
                    }
                }
                ticks++;
                yield return wait;
            }
        }

        if (runningCoroutines.ContainsKey(fadingObj)) 
        {
            StopCoroutine(runningCoroutines[fadingObj]);
            runningCoroutines.Remove(fadingObj);
        }

    }

    private IEnumerator FadeObjectIn(FadingObject fadeObj) 
    {
        float waitTime = 1f / fadeFPS;
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        if (fadeObj.Materials[0].HasProperty("_Color")) // fade material property
        {
            while (fadeObj.Materials[0].color.a < fadeObj.InitialAlpha)
            {
                for (int i = 0; i < fadeObj.Materials.Count; i++)
                {
                    if (fadeObj.Materials[i].HasProperty("_Color"))
                    {
                        fadeObj.Materials[i].color = new Color(
                            fadeObj.Materials[i].color.r,
                            fadeObj.Materials[i].color.g,
                            fadeObj.Materials[i].color.b,
                            Mathf.Lerp(fadedAlpha, fadeObj.InitialAlpha, waitTime * ticks * fadeSpeed)
                            );
                    }
                }
                ticks++;
                yield return wait;
            }
        }

        for (int i = 0; i < fadeObj.Materials.Count; i++)
        {
            if (FadingMode == FadeMode.Fade)
            {
                fadeObj.Materials[i].DisableKeyword("_ALPHABLEND_ON");
            }
            else 
            {
                fadeObj.Materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            fadeObj.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);  // affects both Transparrent and Fade options of unity mats
            fadeObj.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);  // affects both Transparrent and Fade options of unity mats
            fadeObj.Materials[i].SetInt("_ZWrite", 1);  // re-enable Z Writing
            fadeObj.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }

        if (runningCoroutines.ContainsKey(fadeObj))
        {
            StopCoroutine(runningCoroutines[fadeObj]);
            runningCoroutines.Remove(fadeObj);
        }

    }

    private FadingObject GetFadingObjectFromHit(RaycastHit Hit) 
    {
        return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
    }

    private void ClearHits() 
    {
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < Hits.Length; i++)
        {
            Hits[i] = hit;
        }

    }

    public enum FadeMode 
    {
        Transparent,
        Fade
    }

}
