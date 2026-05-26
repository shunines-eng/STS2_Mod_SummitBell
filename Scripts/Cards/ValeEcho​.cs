// 卡名:{空谷回响}
// 效果:{对自身造成1点虚弱,1点易伤,对全体敌方造成5点易伤,5点虚弱,敌方失去1点力量}
// 类型:{技能}
// 牌效偏向:{特效}
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Summitbell.Scripts.Pools;

namespace Summitbell.Scripts.Cards;

// 初始防御
[Pool(typeof(SummitBellCardPool))]
public class ValeEcho : CustomCardModel
{
    // 基础耗能
    private const int energyCost = 0;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Uncommon;
    // 目标类型（AnyEnemy表示任意敌人）None无目标,ALLAllies所有在场者
    private const TargetType targetType = TargetType.AllEnemies;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

	// 为两种debuff分别定义变量
	private const string _vulnerablePowerKey = "EnemyVulnerablePower";
	private const string _weakPowerKey = "EnemyWeakPower";
    private const string _strengthPowerKey = "EnemyStrengthPower";

    // 增加消耗词条
    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[1] { CardKeyword.Exhaust };

    // 卡牌的基础属性,5虚弱+5易伤+1力量
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("EnemyVulnerable", 5m),    // 敌人的易伤
        new DynamicVar("EnemyWeak", 5m),         // 敌人的虚弱
        new DynamicVar("EnemyStrength", -1m)      // 敌人力量变化
    ];
    public override string PortraitPath => $"res://summitbell/images/cards/{nameof(TestCard)}.png";


    public ValeEcho() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        int enemyVulnerable = base.DynamicVars["EnemyVulnerable"].IntValue;
        int enemyWeak = base.DynamicVars["EnemyWeak"].IntValue;
        int selfStrength = base.DynamicVars["EnemyStrength"].IntValue;  // -1
    // 1. 对自身的负面效果
        await PowerCmd.Apply<WeakPower>(ctx,base.Owner.Creature, 1, base.Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(ctx,base.Owner.Creature, 1, base.Owner.Creature, this);
    // 对敌人的负面效果
		foreach (Creature enemy in base.CombatState.HittableEnemies)
		{
			await PowerCmd.Apply<VulnerablePower>(ctx,enemy, enemyVulnerable, base.Owner.Creature, this);
            await PowerCmd.Apply<WeakPower>(ctx,enemy, enemyWeak, base.Owner.Creature, this);
            await PowerCmd.Apply<StrengthPower>(ctx,enemy, selfStrength, base.Owner.Creature, this);
        }
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars["EnemyVulnerable"].UpgradeValueBy(3m);	// 易伤+1
		DynamicVars["EnemyWeak"].UpgradeValueBy(3m);		// 虚弱+1
    }


}