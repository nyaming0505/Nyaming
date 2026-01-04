using System.Collections.Generic;

public static class DrinkJudge
{
    /// <summary>
    /// 주문 레시피와 컵 재료를 비교 (순서 무시)
    /// </summary>
    public static bool IsCorrect(
        List<CupIngredient> orderIngredients,
        List<CupIngredient> cupIngredients)
    {
        // 개수 다르면 바로 실패
        if (orderIngredients.Count != cupIngredients.Count)
            return false;

        // 복사본 생성 (원본 보호)
        List<CupIngredient> orderCopy = new List<CupIngredient>(orderIngredients);
        List<CupIngredient> cupCopy = new List<CupIngredient>(cupIngredients);

        // 정렬 (enum이라 Sort 가능)
        orderCopy.Sort();
        cupCopy.Sort();

        // 하나씩 비교
        for (int i = 0; i < orderCopy.Count; i++)
        {
            if (orderCopy[i] != cupCopy[i])
                return false;
        }

        return true;
    }
}
