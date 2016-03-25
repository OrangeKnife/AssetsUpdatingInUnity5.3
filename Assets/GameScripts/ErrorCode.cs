using System.Collections.Generic;
namespace LOM
{
    public enum ErrorCodeEnum:int
    {
        UpdateGame_Server_RequestError = 1000,
        UpdateGame_Server_ProcessDataError = 1001,
        UpdateGame_Server_ClientVerError = 1002,
        UpdateGame_Server_ClientNeedUpdateApp = 1003,
        UpdateGame_Client_CantReachServer = 1004,
        UpdateGame_Client_CantDownloadData = 1005
    }
    class ErrorCode
    {
        //TODO move this kind of code-string map to a config file
        public static string getErrorString(int ece)
        {
            switch((ErrorCodeEnum)ece)
            {
                case ErrorCodeEnum.UpdateGame_Server_RequestError:
                case ErrorCodeEnum.UpdateGame_Server_ProcessDataError:
                    return "Error: Server can't process your update request <3";
                case ErrorCodeEnum.UpdateGame_Server_ClientVerError:
                    return "Error: Your client verison is broken -,- ";
                case ErrorCodeEnum.UpdateGame_Server_ClientNeedUpdateApp:
                    return "Please update your game through app store <3";
                case ErrorCodeEnum.UpdateGame_Client_CantReachServer:
                    return "You can't reach the server right now :(";
                default:
                return "ErrorCode: " + ece.ToString()+" <3";
            }
            
        }
    }
}
