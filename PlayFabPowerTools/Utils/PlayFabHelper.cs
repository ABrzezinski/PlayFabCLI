using Newtonsoft.Json;
using PlayFab;
using PlayFab.Internal;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabPowerTools.Utils
{
    internal static class PlayFabHelper
    {
        public static string ED_EX_AAD_SCOPE = "448adbda-b8d8-4f33-a1b0-ac58cf44d4c1";
        public static string ED_EX_AAD_SCOPES = ED_EX_AAD_SCOPE + "/plugin";
        public static string ED_EX_AAD_SIGNIN_CLIENTID = "2d99511e-13ec-4b59-99c0-9ae8754f84aa";

        public static string URL = $"https://editor.{PlayFabSettings.staticSettings.ProductionEnvironmentUrl}";

        public static string ED_EX_AAD_SIGNNIN_TENANT = "common";
        public static string AAD_SIGNIN_URL = "https://login.microsoftonline.com/";

        public static string EDEX_NAME = "PlayFabPowerTools CLI";
        public static string EDEX_VERSION = "1.01";
    }
}
