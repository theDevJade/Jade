﻿#region

using System;

#endregion

namespace JadeLib.Features.Stats.Leaderboard;

public record LeaderboardItem(ReferenceHub Owner, float Value, string Message, int Priority, Type Type);