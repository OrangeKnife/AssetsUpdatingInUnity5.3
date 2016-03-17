using System;
using System.Collections.Generic;
using UnityEngine;
namespace LOM
{
    public class Command
    {
        # region private members
        # endregion

        # region Handlers and events
        #endregion

        #region interfaces
        #endregion

        #region Singleton
        private static Command _instance = null;

        public delegate void CommandCall(params object[] paramList);
        private struct SFuncCall
        {
            public int paramCount;
            public CommandCall call;
            public SFuncCall(int paramCountIn, CommandCall callIn)
            {
                paramCount = paramCountIn;
                call = callIn;
            }
            public static bool operator ==(SFuncCall a, SFuncCall b)
            {
                if ((object)a == null || (object)b == null)
                {
                    return false;
                }
                return a.paramCount == b.paramCount && a.call == b.call;
            }
            public static bool operator !=(SFuncCall a, SFuncCall b)
            {
                return !(a == b);
            }
            public override bool Equals(object o)
            {
                return this == (SFuncCall)o;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
       
        private Dictionary<string, SFuncCall> funcTable = new Dictionary<string, SFuncCall>();

        public static Command Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Command();
                }
                return _instance;
            }
        }

        private Command()
        {


        }
        #endregion
        public void Exe(string cmd, object[] cmdparams)
        {
            cmd = cmd.ToLower();
            if (funcTable.ContainsKey(cmd))
            {
                if(cmdparams.Length == 1 && (string)cmdparams[0] == "" && funcTable[cmd].paramCount == 0 //means no param
                    || cmdparams.Length == funcTable[cmd].paramCount)
                    funcTable[cmd].call.Invoke(cmdparams);
                else
                    Debug.Log(cmd+" param count error, requires " + funcTable[cmd].paramCount + " params");
            }
            else
                Debug.Log("no such command: " + cmd);
        }
        public void RegisterCommand(string funcName, int paramCount, CommandCall aFuncCall)
        {
            funcName = funcName.ToLower();
            SFuncCall aNewFuncCall = new SFuncCall(paramCount, aFuncCall);
            if (funcTable.ContainsKey(funcName))
            {
                Debug.LogError("Command:" + funcName + " already exists!");
                return;
            }
            else
                funcTable[funcName] = aNewFuncCall;
        }

    }
}