using Backups.Extra.Tools;

namespace Backups.Extra.Loggers;

public class LogConfiguration
{
    public LogConfiguration(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public bool IsEnabled { get; private set; }

    public void Enable()
    {
        if (IsEnabled)
        {
            BackupExtraException.ConditionNotChanged("enabled");
        }

        IsEnabled = true;
    }

    public void Disable()
    {
        if (!IsEnabled)
        {
            BackupExtraException.ConditionNotChanged("disabled");
        }

        IsEnabled = false;
    }

    public override string ToString()
    {
        if (IsEnabled)
        {
            return $"[{DateTime.Now}]: ";
        }

        return string.Empty;
    }
}