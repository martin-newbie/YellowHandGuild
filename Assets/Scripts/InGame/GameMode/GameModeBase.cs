using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeBase
{
    public abstract bool[] GetResult(bool clear, float gameTime, int retireCount);
    public abstract string GetWave(int stageIdx, int waveIdx);
    public abstract int GetWaveCount(int stageIdx);
    public abstract void OnStageStart();
}

public class NormalMode : GameModeBase
{
    public override bool[] GetResult(bool clear, float gameTime, int retireCount)
    {
        bool[] result = new bool[3];
        result[0] = clear;
        result[1] = clear && gameTime <= 120f; // 추후 수정 가능, 가변 대응 필요함
        result[2] = clear && retireCount == 0; // 이건 고정
        return result;
    }

    public override string GetWave(int stageIdx, int waveIdx)
    {
        return StaticDataManager.GetNormalStageData(stageIdx).wavesInfo[waveIdx];
    }

    public override int GetWaveCount(int stageIdx)
    {
        return StaticDataManager.GetNormalStageData(stageIdx).wavesInfo.Count;
    }

    public override void OnStageStart()
    {
    }
}
