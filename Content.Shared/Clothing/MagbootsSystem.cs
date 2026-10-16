// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._BRatbite.Movement; // Ratbite edit
using Content.Shared.Alert;
using Content.Shared.Atmos.Components;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Gravity;
using Content.Shared.Inventory;
using Content.Shared.Item;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Robust.Shared.Containers;

namespace Content.Shared.Clothing;

public sealed class SharedMagbootsSystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly ItemToggleSystem _toggle = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedGravitySystem _gravity = default!;
    [Dependency] private readonly MagnetizedSystem _magnetized = default!; // Ratbite edit

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MagbootsComponent, ItemToggledEvent>(OnToggled);
        SubscribeLocalEvent<MagbootsComponent, ClothingGotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<MagbootsComponent, ClothingGotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<MagbootsComponent, IsWeightlessEvent>(OnIsWeightless);
        SubscribeLocalEvent<MagbootsComponent, InventoryRelayedEvent<IsWeightlessEvent>>(OnIsWeightless);
    }

    private void OnToggled(Entity<MagbootsComponent> ent, ref ItemToggledEvent args)
    {
        if (!_container.TryGetContainingContainer((ent.Owner, null, null), out var container)) // Ratbite edit begin
            return;

        if (args.Activated)
            _magnetized.AddSource(ent, container.Owner, ent.Comp.RequiresGrid);
        else
            _magnetized.RemoveSource(ent, container.Owner); // Ratbite edit end
    }

    private void OnGotUnequipped(Entity<MagbootsComponent> ent, ref ClothingGotUnequippedEvent args)
    {
        _magnetized.RemoveSource(ent, args.Wearer); // Ratbite edit
    }

    private void OnGotEquipped(Entity<MagbootsComponent> ent, ref ClothingGotEquippedEvent args)
    {
        if (_toggle.IsActivated(ent.Owner)) // Ratbite edit
            _magnetized.AddSource(ent, args.Wearer, ent.Comp.RequiresGrid);
    }

    private void OnIsWeightless(Entity<MagbootsComponent> ent, ref IsWeightlessEvent args)
    {
        if (args.Handled || !_toggle.IsActivated(ent.Owner))
            return;

        // do not cancel weightlessness if the person is in off-grid.
        if (ent.Comp.RequiresGrid && !_gravity.EntityOnGravitySupportingGridOrMap(ent.Owner))
            return;

        args.IsWeightless = false;
        args.Handled = true;
    }

    private void OnIsWeightless(Entity<MagbootsComponent> ent, ref InventoryRelayedEvent<IsWeightlessEvent> args)
    {
        OnIsWeightless(ent, ref args.Args);
    }
}
