using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private Quaternion lastRotation = Quaternion.identity; // Default rotation to avoid NaN errors

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay = TimeManager.Hour + (TimeManager.Minute / 60f);
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        // Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // If the directional light is set then rotate and set it's color
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            // Calculate the target rotation based on time
            Quaternion targetRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));

            // Ensure that targetRotation and lastRotation are valid (not NaN)
            if (targetRotation != Quaternion.identity && !float.IsNaN(targetRotation.x) && !float.IsNaN(targetRotation.y) && !float.IsNaN(targetRotation.z) && !float.IsNaN(targetRotation.w))
            {
                // Smoothly interpolate from the last rotation to the new target rotation
                DirectionalLight.transform.localRotation = Quaternion.Lerp(lastRotation, targetRotation, Time.deltaTime * 2f); // Adjust speed with 2f

                // Save the current rotation for the next frame
                lastRotation = DirectionalLight.transform.localRotation;
            }
            else
            {
                Debug.LogWarning("Invalid quaternion rotation detected. Falling back to default rotation.");
            }
        }
    }

    // Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        // Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        // Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
