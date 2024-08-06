using Robust.Shared.GameObjects;
using Robust.Shared.Timing;
using Robust.Shared.Map;

namespace Content.Server.DayNightCycle
{
    public class DayNightCycleSystem : EntitySystem
    {
        private IGameTiming? _gameTiming;
        private IMapManager? _mapManager;

        public override void Initialize()
        {
            base.Initialize();
            _gameTiming = IoCManager.Resolve<IGameTiming>();
            _mapManager = IoCManager.Resolve<IMapManager>();
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            foreach (var component in EntityManager.EntityQuery<DayNightCycleComponent>())
            {
                component.Update();
            }
        }
    }
}
