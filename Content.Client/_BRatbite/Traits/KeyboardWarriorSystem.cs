using Content.Client.Chat.Managers;
using Content.Client.Cuffs;
using Content.Shared._BRatbite.Traits;
using Content.Shared.Chat;
using Content.Shared.Cuffs.Components;
using Content.Shared.Dataset;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Random.Helpers;
using Robust.Client.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Client._BRatbite.Traits;

public sealed partial class KeyboardWarriorSystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly CuffableSystem _cuffableSystem = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    private TimeSpan _nextUpdate;

    public override void Initialize()
    {
        base.Initialize();
        UpdatesOutsidePrediction = true;
    }

    public override void Update(float _)
    {
        base.Update(_);
        if (_playerManager.LocalEntity is not { } localEntity) return;
        if (
            !TryComp<KeyboardWarriorComponent>(localEntity, out var keyboardWarrior) ||
            !TryComp<CuffableComponent>(localEntity, out var cuffable) ||
            !_cuffableSystem.IsCuffed((localEntity, cuffable)) ||
            !TryComp<MobStateComponent>(localEntity, out var mobState) ||
            mobState.CurrentState != MobState.Alive
) return;
        if (_nextUpdate >= _timing.CurTime) return;
        _nextUpdate = _timing.CurTime + keyboardWarrior.BaseTimeBetweenMessages + TimeSpan.FromSeconds(_random.NextGaussian(σ: keyboardWarrior.MessageStandardDeviation));
        var dataset = _proto.Index<LocalizedDatasetPrototype>(keyboardWarrior.Dataset);
        _chatManager.SendMessage(_random.Pick(dataset), ChatSelectChannel.LOOC);
    }
}
