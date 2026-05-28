using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace Summitbell.Scripts.Cards;

using System.Runtime.CompilerServices;
using Summitbell.Scripts.KeyWords;
using Summitbell.Scripts.Pools;



[Pool(typeof(SummitBellCardPool))]
// 浇筑 - 培育卡牌示例
public class Water : CustomCardModel
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.None;
    private const bool shouldShowInCardLibrary = true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move)];

    public Water() : base(energyCost, type, rarity, targetType, true)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [SummitBellKeyWords.Cultivate];

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        // 1. 获得格挡
        await CreatureCmd.GainBlock(
            ((CardModel)(object)this).Owner.Creature,
            ((CardModel)(object)this).DynamicVars.Block.BaseValue,
            ValueProp.Move,
            cardPlay
        );

        // 2. 查找并触发所有生长卡牌
        await TriggerAllGrowCards(ctx, cardPlay);
    }

    private async Task TriggerAllGrowCards(PlayerChoiceContext ctx, CardPlay originalCardPlay)
    {
        var owner = ((CardModel)(object)this).Owner;
        var allCards = owner.PlayerCombatState.AllCards;
        
        // 获取所有实现了IGrowableCard接口的生长卡牌
        var growableCards = allCards
            .Where(c => c.Keywords.Contains(SummitBellKeyWords.Grow))
            .Where(c => c is IGrowableCard)
            .Where(c => true)  // 只触发不在手牌中的
            .Cast<IGrowableCard>()
            .ToList();

        if (!growableCards.Any())
            return;

        int triggeredCount = 0;
        
        // 触发每张生长卡牌的自身效果
        foreach (var growableCard in growableCards)
        {
            await growableCard.OnCultivateTriggered(ctx, originalCardPlay);
            triggeredCount++;
        }
        
    }

    protected override void OnUpgrade()
    {
        // 升级：增加格挡
        DynamicVars.Block.UpgradeValueBy(3m);
        
        // 或者可以改为：升级后也能触发手牌中的生长卡牌
        // 这需要在TriggerAllGrowCards中修改条件
    }
}