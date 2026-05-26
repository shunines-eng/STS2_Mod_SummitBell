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

// 卡名:{花舞}
// 效果:{造成1点伤害3次,抽一张牌}
// 类型:攻击牌
// 牌效偏向:{过度}

// 注册卡牌。如果要写自定义池看添加人物的开头
[Pool(typeof(SummitBellCardPool))]
public class BlossomBarrage : CustomCardModel
{
    // 基础耗能
    private const int energyCost = 0;
    // 卡牌类型
    private const CardType type = CardType.Attack;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Common;
    // 目标类型（AnyEnemy由玩家指向任意敌人）
    private const TargetType targetType = TargetType.AnyEnemy;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 卡牌的基础属性（例如这里是1点伤害）ValueProp.Move代表效果可被变动,即非静态,可以吃到各种与伤害有
    //这一句的目的是本地化翻译的数据对齐,这里和json文件中{}里的内容对应,因此不能写错,
    //例如DamageVar-->Damge:diff()
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(1, ValueProp.Move),new CardsVar(1)];

    // 添加这一行，指定卡牌立绘路径
    public override string PortraitPath => $"res://summitbell/images/cards/{nameof(TestCard)}.png";


    // 为卡牌添加标签,现在将其归为打击,则其可以吃到打击木偶,完美打击等专门为打击牌增效的增益.
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };

    public BlossomBarrage() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //卡牌空值报错
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属性
            .WithHitCount(3)//攻击次数,(3)
            .FromCard(this) // 伤害来源于这张卡牌
            .Targeting(cardPlay.Target) // 伤害目标是玩家选择的目标
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
    }
    //学习用代码:战士双重打击卡牌的效果描述
	// protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	// {
	// 	ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
	// 	await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this)
	// 		.Targeting(cardPlay.Target)
	// 		.WithHitFx("vfx/vfx_attack_slash")//动画,可选
	// 		.Execute(choiceContext);
	// }
    // 升级后的效果逻辑

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }


}