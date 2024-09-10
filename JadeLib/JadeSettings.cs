namespace JadeLib;

public sealed class JadeSettings
{
    /// <summary>
    ///     Gets the default settings for JadeLib
    /// </summary>
    public static JadeSettings Default { get; } = new()
    {
        UseHintSystem = true,
        InitializeFFmpeg = false,
        UseRoundEvents = false,
        RegisterCommands = true,
        CommandPermission = new CommandPermissions
        {
            SizerPermissions = "jadelib.sizer",
            SpawnRatPermissions = "jadelib.spawnrat",
        },
    };

    /// <summary>
    ///     Gets or sets a value indicating whether JadeLib is supposed to use the built-in hint system.
    /// </summary>
    public bool UseHintSystem { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether JadeLib is supposed to download and initialize FFmpeg.
    /// </summary>
    public bool InitializeFFmpeg { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether JadeLib is supposed to enable Round Events.
    /// </summary>
    public bool UseRoundEvents { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether JadeLib should register built-in Commands.
    ///     <remarks>All commands are registered for remote admin only.</remarks>
    /// </summary>
    public bool RegisterCommands { get; set; }

    /// <summary>
    ///     The permissions for commands if they are registered.
    /// </summary>
    public CommandPermissions CommandPermission { get; set; }

    /// <summary>
    ///     The settings for commands.
    /// </summary>
    public class CommandPermissions
    {
        public string SizerPermissions { get; set; }

        public string SpawnRatPermissions { get; set; }
    }
}