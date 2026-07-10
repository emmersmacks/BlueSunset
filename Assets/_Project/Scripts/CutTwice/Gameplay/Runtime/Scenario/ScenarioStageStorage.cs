using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Scenario
{
    [CreateAssetMenu(fileName = "ScenarioStageStorage", menuName = "CutTwice/Scenario/ScenarioStageStorage")]
    public class ScenarioStageStorage : ScriptableObject
    {
        public ScenarioStage[] Stages;
    }
}