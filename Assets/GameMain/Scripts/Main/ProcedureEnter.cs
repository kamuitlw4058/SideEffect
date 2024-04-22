using System;
using GameFramework;
using GameFramework.Sound;
using Pangoo;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using Pangoo.Core.Services;

namespace SideEffect
{
    public class ProcedureEnter : PangooProcedureBase
    {

        GameMainConfig packageConfig;

        SideEffectMainService container = null;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info($"Enter Idle");
            container = new SideEffectMainService();
            container.Init();
            container.Awake();
            container.Start();

            PangooEntry.Service.SideEffectMainSrv = container;
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            container.Update();
        }

    }
}