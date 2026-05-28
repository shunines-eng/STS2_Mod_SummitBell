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

using Summitbell.Scripts.CardTags;
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
    // 拥有"培育"标签
        // 使用CanonicalTags添加生长标签
    // protected override HashSet<CardTag> CanonicalTags => 
    //     new HashSet<CardKeyword> { PlantTags.Grow };
    public override IEnumerable<CardKeyword> CanonicalKeywords => [PlantTags.Grow];
    // 卡牌属性
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m,  ValueProp.Move)];

    
    public Water() : base(energyCost, type, rarity, targetType, true)
    {
    }
    
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(
            ((CardModel)(object)this).Owner.Creature,  // 目标：当前卡牌所有者
            ((CardModel)(object)this).DynamicVars.Block.BaseValue,  // 格挡值：4
            ValueProp.Move, 
            cardPlay
        );
    }
}