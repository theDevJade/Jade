// <copyright file="JadeSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace JadeLib;

public sealed class JadeSettings
{
    public bool UseHintSystem { get; set; }

    public bool JadeCredit { get; set; }

    public static JadeSettings Default = new()
    {
        UseHintSystem = true,
        JadeCredit = true
    };
}