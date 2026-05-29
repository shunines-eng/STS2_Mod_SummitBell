// 生长卡牌接口
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

public interface IGrowableCard
{
    Task OnCultivateTriggered(PlayerChoiceContext ctx);
}