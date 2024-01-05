using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif


/// <summary>
/// Handle the cycle of Day and Night. Everything that need to change across time will register itself to this handler
/// which will update it when it update (e.g. ShadowInstance, Interpolator etc.).
/// The ticking of that system can be stopped, this is useful e.g. if the game is put in pause (or need to do cutscene
/// etc..)
/// </summary>
[DefaultExecutionOrder(10)]
public class DayCycleHandler : MonoBehaviour
{
    public Transform LightsRoot;

    [Header("Day Light")]
    public Light2D DayLight;
    public Gradient DayLightGradient;

    [Header("Night Light")]
    public Light2D NightLight;
    public Gradient NightLightGradient;

    [Header("Ambient Light")]
    public Light2D AmbientLight;
    public Gradient AmbientLightGradient;

    [Header("RimLights")]
    public Light2D SunRimLight;
    public Gradient SunRimLightGradient;
    public Light2D MoonRimLight;
    public Gradient MoonRimLightGradient;

    [Tooltip("The angle 0 = upward, going clockwise to 1 along the day")]
    public AnimationCurve ShadowAngle;
    [Tooltip("The scale of the normal shadow length (0 to 1) along the day")]
    public AnimationCurve ShadowLength;

    private List<ShadowInstance> m_Shadows = new();
    private List<LightInterpolator> m_LightBlenders = new();

    private void Start()
    {
        TimeManager.Instance.DayCycleHandler = this;
    }

    /// <summary>
    /// We use an explicit ticking function instead of update so the GameManager can potentially freeze or change how
    /// time pass
    /// </summary>
    public void Tick(float ratio)
    {
        UpdateLight(ratio);
    }

    public void UpdateLight(float ratio)
    {
        DayLight.color = DayLightGradient.Evaluate(ratio);
        NightLight.color = NightLightGradient.Evaluate(ratio);

#if UNITY_EDITOR
        //the test between the define will only happen in editor and not in build, as it is assumed those will be set
        //in build. But in editor we may want to test without those set. (those were added later in development so
        //some test scene didn't have those set and we wanted to be able to still test those)
        if (AmbientLight != null)
#endif
            AmbientLight.color = AmbientLightGradient.Evaluate(ratio);

#if UNITY_EDITOR
        if (SunRimLight != null)
#endif
            SunRimLight.color = SunRimLightGradient.Evaluate(ratio);

#if UNITY_EDITOR
        if (MoonRimLight != null)
#endif
            MoonRimLight.color = MoonRimLightGradient.Evaluate(ratio);

        LightsRoot.rotation = Quaternion.Euler(0, 0, 360.0f * ratio);

        UpdateShadow(ratio);
    }

    void UpdateShadow(float ratio)
    {
        var currentShadowAngle = ShadowAngle.Evaluate(ratio);
        var currentShadowLength = ShadowLength.Evaluate(ratio);

        var opposedAngle = currentShadowAngle + 0.5f;
        while (currentShadowAngle > 1.0f)
            currentShadowAngle -= 1.0f;

        foreach (var shadow in m_Shadows)
        {
            var t = shadow.transform;
            //use 1.0-angle so that the angle goes clo
            t.eulerAngles = new Vector3(0, 0, currentShadowAngle * 360.0f);
            t.localScale = new Vector3(1, 1f * shadow.BaseLength * currentShadowLength, 1);
        }

        foreach (var handler in m_LightBlenders)
        {
            handler.SetRatio(ratio);
        }
    }

    public static void RegisterShadow(ShadowInstance shadow)
    {
#if UNITY_EDITOR
        //in the editor when not running, we find the instance manually. Less efficient but not a problem at edit time
        //allow to be able to previz shadow in editor 
        if (!Application.isPlaying)
        {
            var instance = GameObject.FindObjectOfType<DayCycleHandler>();
            if (instance != null)
            {
                instance.m_Shadows.Add(shadow);
            }
        }
        else
        {
#endif
            TimeManager.Instance.DayCycleHandler.m_Shadows.Add(shadow);
#if UNITY_EDITOR
        }
#endif
    }

    public static void UnregisterShadow(ShadowInstance shadow)
    {
#if UNITY_EDITOR
        //in the editor when not running, we find the instance manually. Less efficient but not a problem at edit time
        //allow to be able to previz shadow in editor 
        if (!Application.isPlaying)
        {
            var instance = GameObject.FindObjectOfType<DayCycleHandler>();
            if (instance != null)
            {
                instance.m_Shadows.Remove(shadow);
            }
        }
        else
        {
#endif
            if (TimeManager.Instance?.DayCycleHandler != null)
                TimeManager.Instance.DayCycleHandler.m_Shadows.Remove(shadow);
#if UNITY_EDITOR
        }
#endif
    }

    public static void RegisterLightBlender(LightInterpolator interpolator)
    {
#if UNITY_EDITOR
        //in the editor when not running, we find the instance manually. Less efficient but not a problem at edit time
        //allow to be able to previz shadow in editor 
        if (!Application.isPlaying)
        {
            var instance = FindObjectOfType<DayCycleHandler>();
            if (instance != null)
            {
                instance.m_LightBlenders.Add(interpolator);
            }
        }
        else
        {
#endif
            TimeManager.Instance.DayCycleHandler.m_LightBlenders.Add(interpolator);
#if UNITY_EDITOR
        }
#endif
    }

    public static void UnregisterLightBlender(LightInterpolator interpolator)
    {
#if UNITY_EDITOR
        //in the editor when not running, we find the instance manually. Less efficient but not a problem at edit time
        //allow to be able to previz shadow in editor 
        if (!Application.isPlaying)
        {
            var instance = FindObjectOfType<DayCycleHandler>();
            if (instance != null)
            {
                instance.m_LightBlenders.Remove(interpolator);
            }
        }
        else
        {
#endif
            if (TimeManager.Instance?.DayCycleHandler != null)
                TimeManager.Instance.DayCycleHandler.m_LightBlenders.Remove(interpolator);
#if UNITY_EDITOR
        }
#endif
    }
}

[System.Serializable]
public struct TimeManagerSaveData
{
    public float TimeOfTheDay;
    public int DayOfMonth;
    public int MonthOfYear;
    public int Year;
}