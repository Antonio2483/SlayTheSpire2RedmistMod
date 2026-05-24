using BaseLib.Patches.Content;

namespace Redmist.RedmistCode.Extensions;

public class KeywordExtensions
{

    public static int AGILE = 1001;
    
    public static void registerKeywords()
    {
        int startEnum = 1001;
        List<string> keywords = ["redmist:agile"];

        foreach (string keyword in keywords)
        {
            var currentEnum = startEnum++;
            
            var keywordInfo = new CustomKeywords.KeywordInfo(keyword)
            {
                AutoPosition = AutoKeywordPosition.After,
                RichKeyword = true 
            };
            
            CustomKeywords.KeywordIDs.Add(currentEnum, keywordInfo);
        }
    }
}