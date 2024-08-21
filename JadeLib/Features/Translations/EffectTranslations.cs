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
                $"<color=green>Bodyshot Reduction | {(passer.Intensity / 100f * 100).Dplay()}% | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Poisoned => $"<color=red>Poisoned | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Scp207 => $"<color=#9F2B68>SCP-207 {passer.Intensity}x</color>",
            EffectType.Invisible => $"<color=green>Invisible | {passer.GetTimeLeft()}</color>",
            EffectType.SinkHole => "<color=red>Sinkhole</color>",
            EffectType.DamageReduction =>
                $"<color=green>Damage Reduction | {(passer.Intensity / 100f * 100).Dplay()}% | {passer.GetTimeLeft()}s left.</color>",
            EffectType.MovementBoost =>
                $"<color=green>Movement Boost | {(passer.Intensity / 100f * 100).Dplay()}% | {passer.GetTimeLeft()}s left.</color>",
            EffectType.RainbowTaste => "<color=green>Rainbow Taste</color>",
            EffectType.SeveredHands => "<color=red>Severed Hands</color>",
            EffectType.Stained => $"<color=red>Stained | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Vitality => $"<color=green>Vitality | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Hypothermia => $"<color=blue>Hypothermia | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Scp1853 => "<color=green>SCP-1853</color>",
            EffectType.CardiacArrest => $"<color=red>Cardiac Arrest | {passer.GetTimeLeft()}s left.</color>",
            EffectType.InsufficientLighting => "<color=yellow>Insufficient Lighting</color>",
            EffectType.SoundtrackMute => "<color=yellow>Soundtrack Mute</color>",
            EffectType.SpawnProtected => "<color=yellow>Spawn Protected</color>",
            EffectType.Traumatized => "<color=red>Traumatized</color>",
            EffectType.AntiScp207 => $"<color=#9F2B68>SCP-207? {passer.Intensity}x</color>",
            EffectType.Scanned => "<color=yellow>Scanned</color>",
            EffectType.PocketCorroding => "<color=red>Pocket Corroding</color>",
            EffectType.SilentWalk => $"<color=grey>Silent Walk | {passer.GetTimeLeft()}s left.</color>",
            EffectType.Strangled => "<color=red>Strangled</color>",
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
        return @base.TimeLeft is 0 or < 0 ? "∞" : $"{@base.TimeLeft.Dplay()}";
    }
}