using Content.Shared.Hands;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.UserInterface;
using Robust.Shared.Timing;

namespace Content.Shared._BRatbite.SpyCamera;

public sealed partial class SpyCameraSystem : EntitySystem
{
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SpyCameraComponent, AfterInteractEvent>(OnInteract);
        SubscribeLocalEvent<SpyCameraComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<SpyCameraComponent, GotUnequippedHandEvent>(OnGotUnequippedHand);
        SubscribeLocalEvent<BoundUserInterfaceMessageAttempt>(OnBUIMsgAttempt);
    }

    private void OnBUIMsgAttempt(BoundUserInterfaceMessageAttempt args)
    {
        if (args.Message is OpenBoundInterfaceMessage) return;
        if (IsSpyingOn(args.Actor, args.Target, args.UiKey))
            args.Cancel();
    }
    private void OnInteract(Entity<SpyCameraComponent> ent, ref AfterInteractEvent args)
    {
        if (!args.CanReach || args.Handled || args.Target is not { } target || !TryComp<ActivatableUIComponent>(target, out var activatableUi)) return;
        if (activatableUi.AdminOnly || activatableUi.RequiredItems is not null || activatableUi.InHandsOnly) return;
        ent.Comp.UiKey = activatableUi.Key;
        ent.Comp.TargetEntity = target;
        Dirty(ent);
        _popup.PopupClient(Loc.GetString("spy-camera-interact-success", [("name", Identity.Name(target, EntityManager, args.User))]), args.User);
    }

    private void OnUseInHand(Entity<SpyCameraComponent> ent, ref UseInHandEvent args)
    {
        if (_timing.InPrediction) return;
        if (!_timing.IsFirstTimePredicted) return;
        if (ent.Comp.UiKey is not { } uiKey || ent.Comp.TargetEntity is not { } targetEntity) return;
        if (_ui.IsUiOpen(targetEntity, uiKey, args.User))
        {
            RemoveOpenCamera(args.User, ent);
            _ui.CloseUi(targetEntity, uiKey, args.User);
            return;
        }
        AddOpenCamera(args.User, ent);
        _ui.OpenUi(targetEntity, uiKey, args.User);
    }

    private void OnGotUnequippedHand(Entity<SpyCameraComponent> ent, ref GotUnequippedHandEvent args)
    {
        if (!_timing.IsFirstTimePredicted) return;
        if (ent.Comp.UiKey is not { } uiKey || ent.Comp.TargetEntity is not { } targetEntity) return;
        _ui.CloseUi(targetEntity, uiKey, args.User);
        RemoveOpenCamera(args.User, ent);
    }

    private void RemoveOpenCamera(EntityUid user, Entity<SpyCameraComponent> camera)
    {
        Logger.Debug("Removing camera");
        var cameraUserComp = EnsureComp<SpyCameraUserComponent>(user);
        cameraUserComp.OpenCameras.Remove(camera);
        Dirty(user, cameraUserComp);
    }

    private void AddOpenCamera(EntityUid user, Entity<SpyCameraComponent> camera)
    {
        Logger.Debug("Adding camera");
        var cameraUserComp = EnsureComp<SpyCameraUserComponent>(user);
        if (!cameraUserComp.OpenCameras.Contains(camera))
        {
            cameraUserComp.OpenCameras.Add(camera);
            Dirty(user, cameraUserComp);
        }
    }

    public bool IsSpyingOn(Entity<SpyCameraUserComponent?> user, EntityUid target, Enum uiKey)
    {
        if (!Resolve(user, ref user.Comp, logMissing: false)) return false;
        foreach (var camera in user.Comp.OpenCameras)
        {
            var spyCamera = Comp<SpyCameraComponent>(camera);
            Logger.Debug($"{spyCamera.TargetEntity} {target}");
            if (spyCamera.TargetEntity == target/* && spyCamera.UiKey == uiKey*/)
                return true;
        }
        return false;
    }
}
