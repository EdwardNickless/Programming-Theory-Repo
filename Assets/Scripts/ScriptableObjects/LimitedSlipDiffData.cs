using UnityEngine;

[CreateAssetMenu(fileName = "New Limited Slip Differential", menuName = "Scriptable Objects/Limited Slip Differential")]
public class LimitedSlipDiffData : ScriptableObject
{
    [SerializeField] private bool hasTractionControl;
    [SerializeField] private float defaultForwardStiffness;
    [SerializeField] private float defaultSidewaysStiffness;
    [SerializeField] private float increasedStiffness;

    public bool HasTractionControl {  get { return hasTractionControl; } }
    public float DefaultForwardStiffness { get { return defaultForwardStiffness; } }
    public float DefaultSidewaysStiffness { get { return defaultSidewaysStiffness;} }
    public float IncreasedStiffness { get {  return increasedStiffness; } }
}
