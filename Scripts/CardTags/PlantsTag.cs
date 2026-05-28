using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Summitbell.Scripts.CardTags;
public static class PlantTags
{
    [CustomEnum("GROW")]
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Grow;        // 生长标签
    
    [CustomEnum("CULTIVATE")]
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Cultivate;   // 培育标签
    
    // 培育次数计数器
    public static int CultivateCount = 0;
}