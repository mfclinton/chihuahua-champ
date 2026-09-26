using UnityEngine;

public class Equations
{
    //For every level a strength attribute has, the player can perform 4-6 clicks to complete a rep of (5kg/11.25 lbs * Lvl)
    Weight liftPerLevel = new Weight(5);

    //Gamedev.js is international, so it may be wise to use kg as the default unit
    public static float RoundToDecimal(float value, int decimalPlaces = 0) => Mathf.Round(value * Mathf.Pow(10, decimalPlaces)) * Mathf.Pow(0.1f, decimalPlaces);

    public static float PoundsToKilograms(float value) => RoundToDecimal(value / 2.2f, 1);

    public static float KilogramsToPounds(float value) => RoundToDecimal(value * 2.2f, 1);

    public static float OneRepMaxBrzycki(float weight, int reps) => RoundToDecimal(weight * (36 / (37 - reps)), 1);

    //RPR = Rep Points Rate
    public static float CalcRPR(Attribute stat, Weight weight)
    {
        //For every level a strength attribute has, the player can perform 4-6 clicks to complete a rep of (5kg/11.25 lbs * Lvl)
        Weight liftPerLevel = new Weight(5);

        return (0.2f) * RoundToDecimal((stat.Level * liftPerLevel.value) / weight.value, 4);
    }

    //PWIR = Perceived Weight Increase Rate
    public static float CalcPWIR(Attribute stat, Weight weight, float time)
    {
        //For every level a strength attribute has, the player can perform 4-6 clicks to complete a rep of (5kg/11.25 lbs * Lvl)
        Weight liftPerLevel = new Weight(5);

        return ((0.015f * RoundToDecimal(weight.value / (stat.Level * liftPerLevel.value), 4)) * time) + 0.01f;
    }
}
