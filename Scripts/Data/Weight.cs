using UnityEngine;

[System.Serializable]
public class Weight
{
    public Weight (float value = 0) => SetValue(value);

    public void SetValue(float value)
    {
        kgs = (GameManager.usingKilograms == true ? Equations.RoundToDecimal(value, 1) : Equations.PoundsToKilograms(value));
    }

    // The default unit of measurement is kilograms.
    private float kgs;
    private float lbs => Equations.KilogramsToPounds(kgs);

    public float value => (GameManager.usingKilograms ? kgs : lbs);

    // The Experience Yield will always be calculated in pounds, regardless of the active Unit of Measurement
    public int expYield => Mathf.FloorToInt(lbs * 10f);
}
