// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

namespace JadeLib;

public sealed class JadeSettings
{
    public static JadeSettings Default = new()
    {
        UseHintSystem = true,
        JadeCredit = true
    };

    public bool UseHintSystem { get; set; }

    public bool JadeCredit { get; set; }
}