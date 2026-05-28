// 生长卡牌接口
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

public interface IGrowableCard
{
    // 当被培育触发时调用
    Task OnCultivateTriggered(PlayerChoiceContext ctx, CardPlay originalCardPlay);
}