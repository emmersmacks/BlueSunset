using UnityEngine;

namespace CutTwice.Scenario
{
    [CreateAssetMenu(fileName = "ScenarioStageStorage", menuName = "CutTwice/Scenario/ScenarioStageStorage")]
    public class ScenarioStageStorage : ScriptableObject
    {
        public ScenarioStage[] Stages;
    }
}