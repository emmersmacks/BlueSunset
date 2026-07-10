using System;
using System.Threading;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Scenario
{
    public abstract class ScenarioStage : IInitializable
    {
        [Header("Task Label")]
        public string TaskText;
        public Color TaskColor = Color.black;
        public bool TaskVibration;
        public float TaskShowSpeed = 1f;
        public float TaskShowDelay = 3f;
        public float TaskMoveDistance = 110f;
        
        [Header("Sound")]
        public AudioClip StageSound;

        [Header("Other")] 
        public bool PlayerInputEnabled = true;
        
        [NonSerialized]
        public bool IsActive;
        
        public abstract UniTask InitAsync(CancellationToken ct);
        public abstract bool StageComplete();
        public virtual void StageStart() {}
        public virtual void StageEnd() {}
        public virtual void StageUpdate() {}
    }
}