using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Summitbell.Scripts.Cards;
using Summitbell.Scripts.KeyWords;
// 能力名:{培育形态}
// 效果:{每回合培育1}
// 类型:{能力牌}
// 牌效偏向:{机制终端}

namespace Summitbell.Scripts.Power;
public class CultivatePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext context, Player player)
{
    var combatState = player.PlayerCombatState;
    var allCards = combatState.AllCards;
    
    // 获取所有实现了IGrowableCard接口的生长卡牌
  var growableCards = allCards
            .Where(c => c.Keywords.Contains(SummitBellKeyWords.Grow))
            .Where(c => c is IGrowableCard)
            .Where(c => true)  // 触发全部
            .Cast<IGrowableCard>()
            .ToList();
    
    if (!growableCards.Any())
        return;

    int triggeredCount = 0;

    // 触发每张生长卡牌的自身效果
    foreach (var growableCard in growableCards)
    {
        await growableCard.OnCultivateTriggered(context);
        triggeredCount++;
    }
}
}