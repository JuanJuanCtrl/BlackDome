
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIDustParticles : MonoBehaviour
{
    [Header("Dust Amount")]
    [Range(10, 300)]
    public int particleCount = 100;

    [Header("Particle Size")]
    [Range(0.5f, 5f)]
    public float minSize = 0.8f;

    [Range(0.5f, 8f)]
    public float maxSize = 2.5f;

    [Header("Floating Motion")]
    [Range(1f, 40f)]
    public float minSpeed = 3f;

    [Range(1f, 60f)]
    public float maxSpeed = 12f;

    [Range(0f, 5f)]
    public float wobbleAmount = 1.2f;

    [Range(0.1f, 3f)]
    public float wobbleSpeed = 0.7f;

    [Header("Appearance")]
    [Range(0.05f, 1f)]
    public float minOpacity = 0.15f;

    [Range(0.05f, 1f)]
    public float maxOpacity = 0.65f;

    [Range(0.1f, 5f)]
    public float fadeTime = 1.5f;

    [Header("Drift Direction")]
    public Vector2 windDirection = new Vector2(-0.3f, 1f);

    [Header("Optional")]
    public bool randomizeOnStart = true;
    public bool useUnscaledTime = true;

    private RectTransform area;
    private readonly List<DustParticle> particles =
        new List<DustParticle>();

    private Sprite dustSprite;
    private Texture2D dustTexture;
    private Vector2 normalizedWind;

    private class DustParticle
    {
        public RectTransform rect;
        public Image image;

        public Vector2 position;
        public Vector2 velocity;

        public float size;
        public float opacity;
        public float age;
        public float lifetime;

        public float phase;
        public float wobble;
        public float rotationSpeed;
    }

    private void Awake()
    {
        area = GetComponent<RectTransform>();

        CreateDustTexture();

        normalizedWind = windDirection.sqrMagnitude > 0.001f
            ? windDirection.normalized
            : Vector2.up;
    }

    private void Start()
    {
        CreateParticles();
    }

    private void Update()
    {
        if (area == null || particles.Count == 0)
            return;

        float dt = useUnscaledTime
            ? Time.unscaledDeltaTime
            : Time.deltaTime;

        Rect bounds = area.rect;

        foreach (DustParticle p in particles)
        {
            p.age += dt;

            // Respawn particles after their lifetime expires.
            if (p.age >= p.lifetime)
            {
                ResetParticle(p, bounds, false);
                continue;
            }

            float time = useUnscaledTime
                ? Time.unscaledTime
                : Time.time;

            // Natural floating movement with gentle turbulence.
            float wave = time * wobbleSpeed + p.phase;

            Vector2 drift = new Vector2(
                Mathf.Sin(wave) * p.wobble,
                Mathf.Cos(wave * 0.73f + p.phase) * p.wobble
            );

            p.position += (p.velocity + drift) * dt;

            // Very slow rotation adds subtle variation.
            p.rect.localRotation = Quaternion.Euler(
                0f, 0f, p.phase * 20f + time * p.rotationSpeed
            );

            // Wrap around the edges so the field stays populated.
            float margin = p.size * 2f;

            if (p.position.x > bounds.xMax + margin)
                p.position.x = bounds.xMin - margin;

            if (p.position.x < bounds.xMin - margin)
                p.position.x = bounds.xMax + margin;

            if (p.position.y > bounds.yMax + margin)
                p.position.y = bounds.yMin - margin;

            if (p.position.y < bounds.yMin - margin)
                p.position.y = bounds.yMax + margin;

            p.rect.anchoredPosition = p.position;

            // Fade in and out smoothly.
            float fadeIn = Mathf.Clamp01(p.age / fadeTime);

            float fadeOut = Mathf.Clamp01(
                (p.lifetime - p.age) / fadeTime
            );

            // Smoothstep prevents abrupt opacity changes.
            fadeIn = fadeIn * fadeIn * (3f - 2f * fadeIn);
            fadeOut = fadeOut * fadeOut * (3f - 2f * fadeOut);

            float alpha = Mathf.Min(fadeIn, fadeOut) * p.opacity;

            p.image.color = new Color(1f, 1f, 1f, alpha);
        }
    }

    private void CreateParticles()
    {
        Rect bounds = area.rect;

        for (int i = 0; i < particleCount; i++)
        {
            GameObject obj = new GameObject(
                "Dust Speck",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

            obj.layer = gameObject.layer;
            obj.transform.SetParent(transform, false);

            RectTransform rt = obj.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localScale = Vector3.one;

            Image img = obj.GetComponent<Image>();
            img.sprite = dustSprite;
            img.type = Image.Type.Simple;
            img.preserveAspect = true;
            img.raycastTarget = false;
            img.color = Color.white;

            DustParticle p = new DustParticle
            {
                rect = rt,
                image = img
            };

            particles.Add(p);

            ResetParticle(p, bounds, true);
        }
    }

    private void ResetParticle(
        DustParticle p,
        Rect bounds,
        bool initialSpawn
    )
    {
        p.size = Random.Range(minSize, maxSize);
        p.opacity = Random.Range(minOpacity, maxOpacity);

        p.lifetime = Random.Range(8f, 22f);
        p.age = initialSpawn
            ? Random.Range(0f, p.lifetime)
            : 0f;

        p.phase = Random.Range(0f, Mathf.PI * 2f);
        p.wobble = Random.Range(0.1f, wobbleAmount);
        p.rotationSpeed = Random.Range(-4f, 4f);

        // Each speck receives its own slightly different trajectory.
        Vector2 wind = normalizedWind;

        Vector2 sideways = new Vector2(-wind.y, wind.x);

        p.velocity =
            wind * Random.Range(minSpeed, maxSpeed) +
            sideways * Random.Range(-maxSpeed * 0.3f,
                                     maxSpeed * 0.3f);

        if (initialSpawn)
        {
            p.position = new Vector2(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax)
            );
        }
        else
        {
            // Respawn at the edge opposite the main drift direction.
            float margin = 5f;

            if (Mathf.Abs(wind.x) > Mathf.Abs(wind.y))
            {
                p.position = new Vector2(
                    wind.x > 0f ? bounds.xMin - margin
                                : bounds.xMax + margin,
                    Random.Range(bounds.yMin, bounds.yMax)
                );
            }
            else
            {
                p.position = new Vector2(
                    Random.Range(bounds.xMin, bounds.xMax),
                    wind.y > 0f ? bounds.yMin - margin
                                : bounds.yMax + margin
                );
            }
        }

        p.rect.sizeDelta = new Vector2(p.size, p.size);
        p.rect.anchoredPosition = p.position;
        p.rect.localRotation = Quaternion.identity;

        // Start invisible; the normal fade logic reveals it.
        p.image.color = new Color(1f, 1f, 1f, 0f);
    }

    private void CreateDustTexture()
    {
        const int resolution = 32;

        dustTexture = new Texture2D(
            resolution,
            resolution,
            TextureFormat.RGBA32,
            false
        );

        dustTexture.name = "Procedural Soft Dust";
        dustTexture.filterMode = FilterMode.Bilinear;
        dustTexture.wrapMode = TextureWrapMode.Clamp;

        Color[] pixels = new Color[resolution * resolution];

        // A soft, slightly irregular particle instead of a hard circle.
        float center = (resolution - 1) * 0.5f;
        float radius = resolution * 0.44f;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float nx = (x - center) / radius;
                float ny = (y - center) / radius;

                float distance = Mathf.Sqrt(nx * nx + ny * ny);

                float noise = Mathf.PerlinNoise(
                    x * 0.19f + 12.4f,
                    y * 0.19f + 6.8f
                );

                float irregularRadius =
                    0.92f + (noise - 0.5f) * 0.12f;

                float edge = 1f - Mathf.SmoothStep(
                    irregularRadius - 0.30f,
                    irregularRadius,
                    distance
                );

                float glow = Mathf.Exp(-distance * distance * 5f);

                float alpha = Mathf.Clamp01(
                    edge * 0.55f + glow * 0.45f
                );

                pixels[y * resolution + x] =
                    new Color(1f, 1f, 1f, alpha);
            }
        }

        dustTexture.SetPixels(pixels);
        dustTexture.Apply(false, true);

        dustSprite = Sprite.Create(
            dustTexture,
            new Rect(0, 0, resolution, resolution),
            new Vector2(0.5f, 0.5f),
            resolution
        );

        dustSprite.name = "Procedural Dust Sprite";
    }

    private void OnDestroy()
    {
        foreach (DustParticle p in particles)
        {
            if (p.rect != null)
                Destroy(p.rect.gameObject);
        }

        particles.Clear();

        if (dustSprite != null)
            Destroy(dustSprite);

        if (dustTexture != null)
            Destroy(dustTexture);
    }

    private void OnValidate()
    {
        minSize = Mathf.Min(minSize, maxSize);
        minSpeed = Mathf.Min(minSpeed, maxSpeed);
        minOpacity = Mathf.Min(minOpacity, maxOpacity);
        fadeTime = Mathf.Max(0.1f, fadeTime);
        wobbleSpeed = Mathf.Max(0.1f, wobbleSpeed);
    }
}
