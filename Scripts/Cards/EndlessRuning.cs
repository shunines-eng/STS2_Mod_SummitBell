using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using Summitbell.Scripts.Pools;
using Summitbell.Scripts.Power;
using Summitbell.Scripts.KeyWords;
using BaseLib.Utils;

namespace Summitbell.Scripts.Cards;

[Pool(typeof(SummitBellCardPool))]
public class EndlessRuning : CustomCardModel
{
    private const int energyCost = 2;
    private const CardType type = CardType.Power;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.None;
    private const bool shouldShowInCardLibrary = true;
    
    // 培育层数
    private int cultivateLayers = 1;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
        [SummitBellKeyWords.Cultivate];
    
    // public override string PortraitPath => $"res://summitbell/images/cards/{nameof(EndlessRuning)}.png";
    
    public EndlessRuning() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner?.Creature == null)
            return;
            
        // 施加培育形态能力
        await PowerCmd.Apply<CultivatePower>(
            choiceContext,
            Owner.Creature,     // 目标
            cultivateLayers,    // 层数
            Owner.Creature,     // 施加者
            this    // 卡牌来源
        );
    }
    
    // 升级后，添加固有标签
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}