using UnityEngine;

public class AutoDestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private bool fadeOut = false;
    [SerializeField]
    private float fadeOutTime;

    [SerializeField]
    [Min(1)]
    private float timeToLive = 1f;

    private void Start()
    {
        if (fadeOut)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            new GameTimer(timeToLive - fadeOutTime, () => // attend ce timer avant de lancer celui de fin
            {
                new GameTimer(fadeOutTime, (frame) =>
                {
                    if (renderer == null) return false;
                    renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Lerp(1, 0, frame.Percent));
                    return true;
                });
            });
        }
        new GameTimer(timeToLive, () =>
        {
            Destroy(gameObject);
        });
    }
}
