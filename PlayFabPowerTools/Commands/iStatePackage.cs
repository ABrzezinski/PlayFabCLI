
namespace PlayFabPowerTools
{
    public interface iStatePackage
    {
        void RegisterMainPackageStates(iStatePackage package);

        bool SetState(string line);
        bool Loop();
    }
}
