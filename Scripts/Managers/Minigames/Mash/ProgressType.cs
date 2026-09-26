// ProgressType is to be used to determine what kind of delta is being added to either the rep meter, or the rep time limit.
// This is so that we can give different visual feedback based on the type of delta.
public enum ProgressType
{
    KeyMash,
    Depletion,
    Cooldown
}
