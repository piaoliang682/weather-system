using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TimeOfDayRegistry", menuName = "Environment/Time Registry", order = 2)]
public class TimeOfDayRegistry : ScriptableObject
{
    public List<TimeOfDayDefinition> timesOfDayProfiles = new List<TimeOfDayDefinition>();

    public TimeOfDayDefinition GetTimeByName(string name) =>
        timesOfDayProfiles.Find(t => t.timeName.Equals(name, System.StringComparison.OrdinalIgnoreCase));

    public TimeOfDayDefinition GetRandomTime() =>
        timesOfDayProfiles.Count > 0 ? timesOfDayProfiles[Random.Range(0, timesOfDayProfiles.Count)] : null;
}
