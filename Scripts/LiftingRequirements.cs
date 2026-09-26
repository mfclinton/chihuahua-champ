[System.Serializable]
public class LiftingRequirements
{
    public Weight weight;

    // the default is 20.4 kgs/45 lbs, the weight of a barbell.
    public float weightValue = 20.4f;

    // the default minimum reps.
    public int minimumReps = 10;

    public LiftingRequirements(float barbellWeight, int minimumReps)
    {
        weightValue = barbellWeight;

        if (weight == null)
            weight = new Weight(weightValue);

        this.minimumReps = minimumReps;
    }

    public void Start()
    {
        if (weight == null)
            weight = new Weight(weightValue);
    }

    public float BarbellWeight => weight.value;
    public int MinimumReps => minimumReps;

    public void UpdateWeight(float newBarbellWeight)
    {
        this.weight.SetValue(newBarbellWeight);
    }

    public void UpdateMinimumReps(int newMinimumReps)
    {
        this.minimumReps = newMinimumReps;
    }

}
