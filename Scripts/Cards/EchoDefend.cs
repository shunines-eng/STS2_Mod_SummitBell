using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using Summitbell.Scripts.Pools;

namespace Summitbell.Scripts.Cards;

// 初始防御
[Pool(typeof(SummitBellCardPool))]
public class EchoDefend : CustomCardModel
{
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Basic;
    // 目标类型（AnyEnemy表示任意敌人）None无目标
    private const TargetType targetType = TargetType.None;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 卡牌的基础属性（例如这里是6点格挡）
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m,  ValueProp.Move)];

// 添加这一行，指定卡牌立绘路径
    public override string PortraitPath => $"res://summitbell/images/cards/{nameof(TestCard)}.png";

// 现在将其归为防御,则其可以吃到紧勒等专门为防御牌增效的增益.
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Defend };


    public EchoDefend() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        // 效果1：获得14点格挡
        await CreatureCmd.GainBlock(
            ((CardModel)(object)this).Owner.Creature,  // 目标：当前卡牌所有者
            ((CardModel)(object)this).DynamicVars.Block.BaseValue,  // 格挡值：14
            ValueProp.Move,  // 价值属性：移动
            cardPlay
        );
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4); // 升级后增加4点防御
    }


}