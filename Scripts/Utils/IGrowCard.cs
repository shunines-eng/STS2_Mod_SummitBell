// 生长卡牌的接口
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

public interface IGrowCard
{
    // 当被培育触发时调用
    Task OnCultivateTriggered(PlayerChoiceContext ctx, int cultivateCount);
    
    // 获取当前生长次数
    int GetGrowCount();
    
    // 增加生长
    void AddGrow(int amount);
}