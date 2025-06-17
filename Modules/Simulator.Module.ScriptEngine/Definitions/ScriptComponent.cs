namespace PrismApp.Modules.ScriptEngine.Definitions
{
    public abstract class ScriptComponent
    {
        public dynamic Wmx3Lib { get; set; }
        
        public dynamic wmxlib_cm { get; set; }

        public abstract void Init();

        public abstract void Update();
    }
}
