using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using Robust.Shared.Map;
using Content.Server.Light.Components;

namespace Content.Server.DayNightCycle
{
    [RegisterComponent]
    public partial class DayNightCycleComponent : Component
    {
        [DataField("dayDuration")]
        public TimeSpan DayDuration { get; set; } = TimeSpan.FromMinutes(10);

        [DataField("nightDuration")]
        public TimeSpan NightDuration { get; set; } = TimeSpan.FromMinutes(5);

        [DataField("transitionDuration")]
        public TimeSpan TransitionDuration { get; set; } = TimeSpan.FromMinutes(1);

        private DateTime _lastCycleStart;
        private bool _isDay;

        public void StartCycle()
        {
            _lastCycleStart = DateTime.Now;
            _isDay = true;
        }

        public void Update()
        {
            var currentTime = DateTime.Now;
            var cycleTime = currentTime - _lastCycleStart;

            if (_isDay)
            {
                if (cycleTime >= DayDuration)
                {
                    _isDay = false;
                    _lastCycleStart = currentTime;
                }
            }
            else
            {
                if (cycleTime >= NightDuration)
                {
                    _isDay = true;
                    _lastCycleStart = currentTime;
                }
            }

            var transitionProgress = (float)cycleTime.TotalSeconds / TransitionDuration.TotalSeconds;
            var lightingLevel = _isDay? 1f : 0f;

    if (transitionProgress > 0f && transitionProgress < 1f)
    {
        lightingLevel = _isDay? 1f - (float)transitionProgress : (float)transitionProgress;
    }

    var mapManager = IoCManager.Resolve<IMapManager>();
    var lightComponents = EntityManager.EntityQuery<Content.Server.Light.Components.DynamicLightComponent>();

    foreach (var lightComponent in lightComponents)
    {
        lightComponent.Enabled = _isDay;
        lightComponent.Range = lightingLevel;
    }
}
    }
}
