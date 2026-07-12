namespace MonLauncherJeux.UI.Animations;

/// <summary>
/// Gestionnaire des animations fluides pour la UI
/// </summary>
public sealed class AnimationManager
{
    private readonly List<Animation> _activeAnimations = new();
    private readonly System.Windows.Forms.Timer _animationTimer;
    private bool _animationsEnabled = true;

    public bool AnimationsEnabled
    {
        get => _animationsEnabled;
        set => _animationsEnabled = value;
    }

    public AnimationManager()
    {
        _animationTimer = new System.Windows.Forms.Timer { Interval = 16 };
        _animationTimer.Tick += (_, _) => UpdateAnimations();
    }

    /// <summary>
    /// Lance une animation
    /// </summary>
    public void Animate(Control control, AnimationType type, int duration = 300, Action<float>? onProgress = null, Action? onComplete = null)
    {
        if (!_animationsEnabled)
        {
            onComplete?.Invoke();
            return;
        }

        var animation = new Animation(control, type, duration, onProgress, onComplete);
        _activeAnimations.Add(animation);

        if (_activeAnimations.Count == 1)
            _animationTimer.Start();
    }

    /// <summary>
    /// Met a jour toutes les animations actives
    /// </summary>
    private void UpdateAnimations()
    {
        for (int i = _activeAnimations.Count - 1; i >= 0; i--)
        {
            var animation = _activeAnimations[i];
            if (animation.Update())
            {
                _activeAnimations.RemoveAt(i);
            }
        }

        if (_activeAnimations.Count == 0)
            _animationTimer.Stop();
    }

    /// <summary>
    /// Arrete toutes les animations
    /// </summary>
    public void StopAll()
    {
        _activeAnimations.Clear();
        _animationTimer.Stop();
    }
}

public enum AnimationType
{
    FadeIn,
    FadeOut,
    SlideInLeft,
    SlideInRight,
    SlideInTop,
    SlideInBottom,
    ZoomIn,
    ZoomOut,
    Pulse,
    Shake
}

/// <summary>
/// Represente une animation unique
/// </summary>
public sealed class Animation
{
    private readonly Control _control;
    private readonly AnimationType _type;
    private readonly int _duration;
    private readonly Action<float>? _onProgress;
    private readonly Action? _onComplete;
    private int _elapsedTime;
    private float _progress;

    public Animation(Control control, AnimationType type, int duration, Action<float>? onProgress, Action? onComplete)
    {
        _control = control;
        _type = type;
        _duration = duration;
        _onProgress = onProgress;
        _onComplete = onComplete;
    }

    /// <summary>
    /// Met a jour l'animation. Retourne true si l'animation est terminees.
    /// </summary>
    public bool Update()
    {
        _elapsedTime += 16;
        _progress = Math.Min((float)_elapsedTime / _duration, 1f);

        // Easing sinusoidal
        var easedProgress = (float)(Math.Sin((_progress - 0.5f) * Math.PI) / 2 + 0.5f);

        ApplyAnimation(easedProgress);
        _onProgress?.Invoke(easedProgress);

        if (_progress >= 1f)
        {
            _onComplete?.Invoke();
            return true;
        }

        return false;
    }

    private void ApplyAnimation(float progress)
    {
        switch (_type)
        {
            case AnimationType.FadeIn:
                _control.Visible = true;
                break;

            case AnimationType.FadeOut:
                if (progress >= 0.9f)
                    _control.Visible = false;
                break;

            case AnimationType.Pulse:
                // Pulse est optionnel avec Visible
                break;
        }
    }
}
