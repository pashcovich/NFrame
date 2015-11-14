﻿//-----------------------------------------------------------------------
// <copyright file="NFClusterClientModule.cs">
//     Copyright (C) 2015-2015 lvsheng.huang <https://github.com/ketoo/NFrame>
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFrame
{
    public class NFClusterClientModule : NFILogicModule
    {
        public enum ClusterClientState
        {
            ADDING,
            CONNECTING,
            CONNECTED,
            DISCONNECT,
            RECONNECT,
        }

        public class ConnectObjectData
        {
            public ConnectObjectData()
            {
            }

            public float fMaxReconnectTime;//重连时间
            public DateTime xNextTriggerTime = new DateTime();//下次重连时间

            public long nServerID;
            public NFServer_def.NF_SERVER_TYPES meServerType;
            public string mstrIP;
            public string mstrDns;
            public string mstrAuth;

            public long mnPort;
            public long mnAreaID;
            public NFNetModule xNetModule;
            public NFClusterClientModule.ClusterClientState eState;
        }


        public NFNetHandler GetNetHandler()
        {
            return mxNetHandler;
        }
        public override void Execute()
        {
            foreach(var kv in mxNetDic)
            {
                switch(kv.Value.eState)
                {
                    case ClusterClientState.ADDING:
                        {
                            //kv.Value.xNetModule
                        }
                        break;
                    case ClusterClientState.CONNECTING:
                        {
                            kv.Value.xNetModule.Execute();
                        }
                        break;
                    case ClusterClientState.CONNECTED:
                        {
                            kv.Value.xNetModule.Execute();
                        }
                        break;
                    case ClusterClientState.DISCONNECT:
                        {
                            kv.Value.eState = ClusterClientState.RECONNECT;
                        }
                        break;
                    case ClusterClientState.RECONNECT:
                        {
                            //kv.Value.xNetModule
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void AddNetInfo(ConnectObjectData xData)
        {
            if(null == xData)
            {
                return;
            }

            if (mxNetDic.ContainsKey(xData.nServerID))
            {
                //或者，更新ip等
                return;
            }

            xData.eState = ClusterClientState.ADDING;
            mxNetDic.Add(xData.nServerID, xData);
        }

        private readonly NFNetHandler mxNetHandler = new NFNetHandler();
        private readonly Dictionary<long, ConnectObjectData> mxNetDic = new Dictionary<long, ConnectObjectData>();
    }
}
