using System;
using GameFramework;
using GameFramework.Sound;
using Pangoo;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using Pangoo.Core.Services;


public class ProcedureIdle : PangooProcedureBase
{

    bool initBool;
    GameMainConfig packageConfig;
    bool Created;

    MainService container = null;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        Log.Info($"Enter Idle");
        container = new MainService();
        container.Init();
        container.Awake();
        container.Start();

        PangooEntry.Service.mainService = container;
    }


    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        container.Update();
    }

}