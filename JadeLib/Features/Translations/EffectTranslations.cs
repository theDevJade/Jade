// <copyright file="EffectTranslations.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#region

using System;
using CustomPlayerEffects;
using Exiled.API.Enums;
using JadeLib.Features.Extensions;
using JadeLib.Features.UtilityClasses;

#endregion

namespace JadeLib.Features.Translations;

public sealed class EffectTranslations : EnumTranslation<EffectTranslations, EffectType, string, StatusEffectBase>
{
    protected override string GetNonStatic(ref EffectType @enum, StatusEffectBase passer = null)
    {
        if (passer == null)
        {
            throw new NullReferenceException("For effects, passer must be valid.");
        }

        return @enum switch
        {
            EffectType.AmnesiaItems => "<color=red>Amnesia</color>",
            EffectType.AmnesiaVision => "<color=red>Amnesia</color>",
            EffectType.Asphyxiated => $"<color=red>Asphyxiated | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Bleeding => $"<color=red>Bleeding | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Blinded => $"<color=red>Blinded | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Burned => $"<color=red>Burned | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Concussed => $"<color=red>Concussed | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Corroding => $"<color=red>Corroding | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Deafened => $"<color=red>Deafened | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Decontaminating => "<color=red>Decontaminating</color>",
            EffectType.Disabled => $"<color=red>Disabled | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Ensnared => $"<color=red>Ensnared | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Exhausted => $"<color=red>Exhausted | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Flashed => $"<color=red>Flashed | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Hemorrhage => $"<color=red>Hemorrhage | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Invigorated => $"<color=green>Invigorated | {passer.GetTimeLeft()}s left.</color>",
            EffectType.BodyshotReduction =>
                $"<color=green>Bodyshot Reduction | {passer.Intensity} | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Poisoned => $"<color=red>Poisoned | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Scp207 => "<color=blue>SCP-207</color>",
            EffectType.Invisible => "<color=green>Invisible</color>",
            EffectType.SinkHole => "<color=red>Sinkhole</color>",
            EffectType.DamageReduction => $"<color=green>Damage Reduction | {passer.GetTimeLeft()}s left.</color>",
            EffectType.MovementBoost => $"<color=green>Movement Boost | {passer.GetTimeLeft()}s left.</color>",
            EffectType.RainbowTaste => "<color=rainbow>Rainbow Taste</color>",
            EffectType.SeveredHands => "<color=red>Severed Hands</color>",
            EffectType.Stained => $"<color=red>Stained | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Vitality => $"<color=green>Vitality | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Hypothermia => $"<color=blue>Hypothermia | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Scp1853 => "<color=red>SCP-1853</color>",
            EffectType.CardiacArrest => $"<color=red>Cardiac Arrest | {passer.GetTimeLeft()}s left.</color>",
            EffectType.InsufficientLighting => "<color=red>Insufficient Lighting</color>",
            EffectType.SoundtrackMute => "<color=grey>Soundtrack Mute</color>",
            EffectType.SpawnProtected => "<color=green>Spawn Protected</color>",
            EffectType.Traumatized => "<color=red>Traumatized</color>",
            EffectType.AntiScp207 => "<color=blue>SCP-207?</color>",
            EffectType.Scanned => "<color=yellow>Scanned</color>",
            EffectType.PocketCorroding => "<color=red>Pocket Corroding</color>",
            EffectType.SilentWalk => $"<color=grey>Silent Walk | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Strangled => $"<color=red>Strangled | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Ghostly => "<color=white>Ghostly</color>",
            EffectType.FogControl => "<color=grey>Fog Control</color>",
            EffectType.Slowness => $"<color=red>Slowness | {passer.GetTimeLeft()}s left.</color>",
            _ => "None"
        };
    }
}

public static class EffectExtensions
{
    public static string GetTimeLeft(this StatusEffectBase @base)
    {
        return @base.TimeLeft == 0 ? "∞" : $"{@base.TimeLeft.Dplay()}";
    }
}