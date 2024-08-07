using Exiled.API.Interfaces;

namespace JadeLib
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
    }
}