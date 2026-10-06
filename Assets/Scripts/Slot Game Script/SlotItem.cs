using UnityEngine;
using System.Collections;
using gm.api.domain.Models;
using System.Collections.Generic;

public enum SlotItemType
{
    Wild,
    Scatter,
    Normal,
    Bonus
}

public class SlotItem : MonoBehaviour
{
    public AudioClip[] audios;
    public Texture2D[] slotItemTextures;
    public Texture2D[] slotItemBlurryTextures;

    public Sprite[] slotItemBlurrySprites;
    private ColumnScript myColumnScript;
    public int indexInColumn;
    public int animationIndex = 0;
    private Transform myTransform;
    internal bool spin = false;
    public SlotItemType itemType = SlotItemType.Normal;
    internal bool luckyItem = false;
    internal float luckChance;
    public static float counter;
    public SlotItemEffects myEffectScript;
    private bool once;
    private int previouscolumn;
    private Vector3 InitialScale;
    private Vector2 originalTextureScale;
    private Vector2 originalTextureOffset;
    private bool spriteVisualApplied;
    public ColumnScript ParentColumn;
    public int MyColumnIndex;
    public GameObject PayBoxPrefab;
    public float randomNo;
    public int[] demoProbs = new int[] { 10, 20, 30, 38, 46, 54, 60, 65, 75, 90, 95, 100 };

    void Start()
    {
        ParentColumn = GetComponentInParent<ColumnScript>();
        MyColumnIndex = ParentColumn.columnIndex;
        InitialScale = transform.localScale;
        once = false;
        myTransform = transform;
        myColumnScript = transform.parent.GetComponent<ColumnScript>();
        Material material = GetComponent<Renderer>().material;
        originalTextureScale = material.mainTextureScale;
        originalTextureOffset = material.mainTextureOffset;
        SetNewAnim();
    }

    void Update()
    {
        if (!spin)
            return;

        // Itween Code To Reset Slot Item Position..
        if (myColumnScript.isSpinning == false)
        {
            if (indexInColumn < LineManager.instance.Items_In_Column)
                SetItemOnProb();

            if (GameOperations.instance.HasExpandedWild)
            {
                if (itemType == SlotItemType.Wild && !ParentColumn.IsColumnWild && indexInColumn == 0)
                {
                    ParentColumn.IsColumnWild = true;
                    ShowWildExpnd();
                }
                else
                    CheckForWildColumn();
            }
            //  iTween.ShakeScale(gameObject, new Vector3(0.2f, 0.2f, 1f), 0.3f);

            iTween.Defaults.easeType = ColumnManager.instance.easetype;
            float stoppedItemY = myColumnScript.transform.position.y - 0.2f - indexInColumn * SlotManager.instance.verticalGap;
            iTween.MoveTo(gameObject, new Vector3(transform.position.x, stoppedItemY, transform.position.z), ColumnManager.instance.setTime);
            spin = false;


            if (itemType == SlotItemType.Bonus && !once && indexInColumn < LineManager.instance.Items_In_Column)
            {
                once = true;


                iTween.ScaleFrom(gameObject, gameObject.transform.localScale * 1.5f, 1.5f);

                if (UltimateBonusRecorder.Instance)
                {
                    GetComponent<BonusSlotitem>().BonusItemPosandReward();

                }

            }

            if (itemType == SlotItemType.Scatter && indexInColumn < LineManager.instance.Items_In_Column)
            {

                if (ParentColumn.columnIndex < 4 && GameOperations.instance.scattersMatched > 1)
                {
                    ColumnManager.instance.LongSpin = true;
                }

            }

            if (itemType == SlotItemType.Wild && indexInColumn < LineManager.instance.Items_In_Column)
            {
                if (!SoundFxManager.instance.BonusItemAppear.isPlaying)
                    SoundFxManager.instance.BonusItemAppear.Play();
                iTween.ScaleFrom(gameObject, gameObject.transform.localScale * 1.5f, 0.6f);
                if (GameOperations.instance.WildAsMultiplier && SlotManager.instance.spinType == "bonus")
                {

                    iTween.PunchScale(GameOperations.instance.collectedmesh.gameObject, GameOperations.instance.collectedmesh.transform.localScale * 1.2f, 1);
                    GameOperations.instance.collect++;
                    GameOperations.instance.CalculateFreespinMultiplier();



                }
            }
            //}
            return;
        }


        // Move Item Down-ward..
        myTransform.position -= new Vector3(0, SlotManager.instance.spinSpeed * Time.deltaTime, 0);
        Vector3 newPos = transform.position;
        newPos.z = 1;
        transform.position = newPos;
        // Move Last Item To First Position..
        if (myTransform.position.y < -2f)
        {
            myColumnScript.MoveLastItem();
            myTransform.position = new Vector3(myTransform.position.x, 3f, myTransform.position.z);
            SetNewAnim();


        }

    }// End Update();


    void CheckForWildColumn()
    {
        if (ParentColumn.IsColumnWild)
        {
            itemType = SlotItemType.Wild;
            animationIndex = 9;
            ApplyCurrentVisual();

        }
    }
    public void ShowWildExpnd()
    {

        GameObject WildClone = (GameObject)Instantiate(ParentColumn.WildExpandedPrefab, ParentColumn.WildExpandedPrefab.transform.position, Quaternion.identity);
        ColumnManager.instance.wildclones.Add(WildClone);
        if (!SoundFxManager.instance.WildExpandedSound.isPlaying)
        {
            SoundFxManager.instance.WildExpandedSound.Play();
        }
    }
    /// Set Random Animation On Slot Item..
    public static bool onceCheck = false;

    // Zero Win settings in Slots Matching

    internal void SetItemOnProb()
    {
        randomNo = Random.Range(0, 1000);
        if (randomNo <= ProbabilityManager.instance.ItemsWeightsRange[0])
        {
            animationIndex = 0;
            itemType = SlotItemType.Normal;
            ApplyCurrentVisual();
            goto checking;
        }

        for (int i = 1; i < 12; i++)
        {
            if (randomNo > ProbabilityManager.instance.ItemsWeightsRange[i - 1] && randomNo <= ProbabilityManager.instance.ItemsWeightsRange[i])
            {
                animationIndex = i;
                if (animationIndex < 9)
                    itemType = SlotItemType.Normal;
                else if (animationIndex == 9)
                    itemType = SlotItemType.Wild;

                else if (animationIndex == 10)
                    itemType = SlotItemType.Scatter;
                else
                {
                    if (!GameOperations.instance.NoBonusAtAll)
                        itemType = SlotItemType.Bonus;
                    else
                    {
                        itemType = SlotItemType.Normal;
                        animationIndex = Random.Range(0, 9);
                    }
                }
                if (itemType == SlotItemType.Scatter && GameOperations.instance.BonusMatched >= 3 && indexInColumn < LineManager.instance.Items_In_Column)
                {
                    itemType = SlotItemType.Normal;
                    animationIndex = 8;
                }
                else
                    if (itemType == SlotItemType.Bonus && GameOperations.instance.scattersMatched >= 3 && indexInColumn < LineManager.instance.Items_In_Column)
                    {
                        itemType = SlotItemType.Normal;
                        animationIndex = 7;
                    }
                break;
            }
        }
        if (animationIndex == 9 && ParentColumn.columnIndex == 0)
        {
            animationIndex = Random.Range(0, 9);
            itemType = SlotItemType.Normal;

        }
        else
            if (SlotManager.instance.IsFreeSpinsEnabled && animationIndex == 11)
            {
                animationIndex = Random.Range(0, 9);
                itemType = SlotItemType.Normal;
            }

        if (ParentColumn.columnIndex == 0)
        {
            if (indexInColumn < LineManager.instance.Items_In_Column)
                ParentColumn.column0indexes[indexInColumn] = animationIndex;

        }
    checking:

        // ====== BACKEND RTP ENGINE CONTROLS ALL WIN/LOSS DECISIONS ======
        // The 'chance' value now comes from the backend RTP engine via ColumnManager
        // Backend determines: Zero/VeryLow/Low/High/VeryHigh based on admin RTP settings
        // All MaxRTPStatus checks removed - backend handles max profit enforcement

        // ====== ALL SPINS USE BACKEND DETERMINISTIC OUTCOME ======
        // Whether normal spin or free spin, backend controls the result
        ApplyBackendOutcome();

        // ====== PREVENT BONUS SYMBOLS WHEN BACKEND DISALLOWS ======
        // If backend says allowBonus=false, convert bonus symbols to regular
        // This prevents showing a "winning" bonus combination that doesn't trigger

        
        // if (itemType == SlotItemType.Bonus && RtpEngineManager.Instance != null && !RtpEngineManager.Instance.IsBonusAllowed())
        // {
        //     // Convert to regular symbol - don't show bonus if it won't trigger
        //     animationIndex = Random.Range(0, 9);
        //     itemType = SlotItemType.Normal;
        // }

        ApplyCurrentVisual();

        if (itemType == SlotItemType.Bonus && indexInColumn < LineManager.instance.Items_In_Column)
        {

            GameOperations.instance.BonusMatched += 1;
        }
        else if (itemType == SlotItemType.Scatter && indexInColumn < LineManager.instance.Items_In_Column)
        {
            GameOperations.instance.scattersMatched += 1;
        }
    }
    private void VeryLowWin()
    {
        if (ParentColumn.columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0])
            {
                animationIndex = ColumnManager.instance.lowitem;
                itemType = SlotItemType.Normal;
            }
        }
        else
        {
            if (ParentColumn.columnIndex == 1)
            {
                if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1])
                {
                    animationIndex = ColumnManager.instance.lowitem;
                    itemType = SlotItemType.Normal;
                }
            }
            else if (ParentColumn.columnIndex == 2)
            {
                if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2])
                {
                    animationIndex = ColumnManager.instance.lowitem;
                    itemType = SlotItemType.Normal;
                }
            }
            else if (ParentColumn.columnIndex == 3)
            {
                if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3])
                {
                    animationIndex = Random.RandomRange(0, 5);
                    itemType = SlotItemType.Normal;
                }
            }
            else if (ParentColumn.columnIndex == 4)
            {
                if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4])
                {
                    animationIndex = Random.RandomRange(0, 5);
                    itemType = SlotItemType.Normal;
                }
            }
        }
    }
    /// <summary>
    /// Apply the backend-determined outcome using DETERMINISTIC symbol placement.
    /// Uses ColumnManager's pre-calculated configuration (targetMatchCount, targetSymbol)
    /// to place symbols that produce the target multiplier.
    /// </summary>
    private void ApplyBackendOutcome()
    {
        // Use deterministic approach based on backend's suggestedMultiplier
        // Debug.Log($"[SlotItem] ApplyBackendOutcome: backendWon={ColumnManager.instance.backendWon}, suggestedMult={ColumnManager.instance.suggestedMultiplier}, targetMatch={ColumnManager.instance.targetMatchCount}, targetSym={ColumnManager.instance.targetSymbol}");

        if (!ColumnManager.instance.backendWon)
        {
            // Backend says LOSS - ensure no winning combinations
            // Debug.Log($"[SlotItem] Backend says LOSS - calling Zerowin()");
            Zerowin();
            return;
        }

        // Backend says WIN - use deterministic symbol placement
        // Debug.Log($"[SlotItem] Backend says WIN - calling ApplyDeterministicWin()");
        ApplyDeterministicWin();
    }

    /// <summary>
    /// Place symbols deterministically to match the backend's target multiplier.
    /// Uses ColumnManager's targetMatchCount and targetSymbol.
    /// </summary>
    private void ApplyDeterministicWin()
    {
        int matchCount = ColumnManager.instance.targetMatchCount;
        int targetSym = ColumnManager.instance.targetSymbol;
        int paylineCount = ColumnManager.instance.targetPaylineCount;

        // Determine which paylines to use
        LineItem payline1 = ColumnManager.instance.randompayline;
        LineItem payline2 = ColumnManager.instance.randompayline2;

        if (payline1 == null)
        {
            Debug.LogError($"[SlotItem] CRITICAL: randompayline is null! Cannot apply deterministic win.");
            return;
        }

        // Check if this slot is on a winning payline and should be a matching symbol
        bool shouldMatch = false;
        int paylineRowForThisColumn = payline1.rowIndex[ParentColumn.columnIndex];

        // First payline
        if (ParentColumn.columnIndex < matchCount)
        {
            if (indexInColumn == paylineRowForThisColumn)
            {
                shouldMatch = true;
                // Debug.Log($"[SlotItem] MATCH: col={ParentColumn.columnIndex}, row={indexInColumn}, paylineRow={paylineRowForThisColumn}, setting symbol={targetSym}");
            }
        }

        // Second payline (for very high wins)
        if (paylineCount >= 2 && ParentColumn.columnIndex < matchCount && payline2 != null)
        {
            if (indexInColumn == payline2.rowIndex[ParentColumn.columnIndex])
            {
                shouldMatch = true;
            }
        }

        if (shouldMatch)
        {
            // This slot should be the matching symbol
            animationIndex = targetSym;
            itemType = SlotItemType.Normal;
        }
        else if (ParentColumn.columnIndex == matchCount && matchCount < 5)
        {
            // Column after the match chain - MUST NOT match to cap the win
            if (indexInColumn == payline1.rowIndex[ParentColumn.columnIndex] ||
                (paylineCount >= 2 && payline2 != null && indexInColumn == payline2.rowIndex[ParentColumn.columnIndex]))
            {
                // Ensure this symbol doesn't extend the match
                int attempts = 0;
                while ((animationIndex == targetSym || animationIndex == 9) && attempts < 50) // 9 = Wild
                {
                    animationIndex = Random.Range(0, 8);
                    attempts++;
                }
                itemType = SlotItemType.Normal;
            }
        }
        // Other positions keep their random symbols
    }

    private void Optimize_gata()
    {
        if (ParentColumn.columnIndex == 1)
        {
            while (ColumnManager.instance.columnScripts[0].column0indexes.Contains(animationIndex))
            {
                animationIndex = Random.Range(0, 9);
                itemType = SlotItemType.Normal;
            }
        }
        // Use backend outcome
        ApplyBackendOutcome();
        //if (ColumnManager.instance.chance <= 0.1f)
        //        Zerowin();
        //    else if (ColumnManager.instance.chance <= 0.5f)
        //        VeryLowWin();
        //    else if (ColumnManager.instance.chance <= 0.8f)
        //        LowWin();
        //    else if ( ColumnManager.instance.chance <= 0.9f)
        //        HighWin();
        //    else VeryHighWin();

    }

    private void Zerowin()
    {
        // RANDOMIZED LOSS: Prevent winning combinations while keeping natural symbol variation
        //
        // For standard slot games (3-5+ reels), wins require 3+ matching symbols from left to right.
        // To guarantee NO win, we only need to "break the chain" at column 2.
        // - Columns 0 and 1: Keep their random symbols from probability selection
        // - Column 2: Must NOT match any symbol from column 0 (prevents 3-of-a-kind)
        // - Columns 3+: Keep random symbols, just convert special symbols to regular
        //
        // This preserves natural symbol variation while ensuring backend-controlled losses.

        int columnIndex = ParentColumn.columnIndex;

        if (columnIndex == 2)
        {
            // Column 2 is the PRIMARY chain breaker
            // Must NOT contain any symbol from column 0 to prevent 3+ of a kind on any payline
            var column0Symbols = ColumnManager.instance.columnScripts[0].column0indexes;

            int attempts = 0;
            while (attempts < 100)
            {
                bool matchesColumn0 = column0Symbols != null &&
                                      column0Symbols.Count > 0 &&
                                      column0Symbols.Contains(animationIndex);
                bool isSpecialSymbol = animationIndex >= 9; // Wild(9), Scatter(10), Bonus(11)

                if (!matchesColumn0 && !isSpecialSymbol)
                    break; // Found a safe symbol that won't create a win

                // Pick a new random symbol (regular symbols only: 0-8)
                animationIndex = Random.Range(0, 9);
                itemType = SlotItemType.Normal;
                attempts++;
            }
        }
        else if (columnIndex > 2)
        {
            // Columns 3, 4, etc. - keep random symbol from probability selection
            // Just ensure no special symbols appear during a loss spin
            if (animationIndex >= 9)
            {
                animationIndex = Random.Range(0, 9);
                itemType = SlotItemType.Normal;
            }
        }
        else
        {
            // Columns 0 and 1 - keep their random symbols but convert specials
            // This maintains natural variation while preventing bonus/scatter triggers on loss
            if (animationIndex >= 9)
            {
                animationIndex = Random.Range(0, 9);
                itemType = SlotItemType.Normal;
            }
        }

        // Final safety: ensure we're displaying a regular symbol type
        itemType = SlotItemType.Normal;
    }

    private void LowWin()
    {
        if (ParentColumn.columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0])
            {
                animationIndex = ColumnManager.instance.lowitem;
                itemType = SlotItemType.Normal;
            }

        }
        else if (ParentColumn.columnIndex == 1)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1])
            {
                if (Random.value > 0.5 && !GameOperations.instance.WildAsMultiplier)
                {
                    animationIndex = 9;
                    itemType = SlotItemType.Wild;
                }
                else
                {
                    animationIndex = Random.Range(5, 10);
                    if (animationIndex < 9)
                        itemType = SlotItemType.Normal;
                    else
                        itemType = SlotItemType.Wild;
                }
            }
        }
        else if (ParentColumn.columnIndex == 2)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2])
            {
                animationIndex = ColumnManager.instance.lowitem;
                itemType = SlotItemType.Normal;
            }

        }
        else if (ParentColumn.columnIndex == 3)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3])
            {
                animationIndex = Random.Range(6, 10);
                if (animationIndex < 9)
                    itemType = SlotItemType.Normal;
                else
                    itemType = SlotItemType.Wild;
            }
        }
        else if (ParentColumn.columnIndex == 4)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4])
            {
                animationIndex = Random.RandomRange(0, 5);
                itemType = SlotItemType.Normal;
            }
        }
    }
    private void HighWin()
    {
        // DETERMINISTIC HIGH WIN: Guarantee 4-of-a-kind
        // Columns 0,1,2,3 = matching symbols
        // Column 4 = break the chain (different symbol)

        GameSocketManager.Instance.spins_counter = 0;

        if (ParentColumn.columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }
        }
        else if (ParentColumn.columnIndex == 1)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }
        }
        else if (ParentColumn.columnIndex == 2)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }
        }
        else if (ParentColumn.columnIndex == 3)
        {
            // Column 3: MUST match to complete 4-of-a-kind
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }
        }
        else if (ParentColumn.columnIndex == 4)
        {
            // Column 4: MUST NOT match to cap at 4-of-a-kind (prevent 5-of-a-kind)
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4])
            {
                int highItem = ColumnManager.instance.hightitem;
                int attempts = 0;
                while ((animationIndex == highItem || animationIndex == 9) && attempts < 50)
                {
                    animationIndex = Random.Range(0, 8); // Exclude Wild (9)
                    attempts++;
                }
                itemType = SlotItemType.Normal;
            }
        }
    }


    private void VeryHighWin()
    {

        GameSocketManager.Instance.spins_counter = 0;


        if (ParentColumn.columnIndex == 0)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[0] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[0] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[0])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }

        }
        else if (ParentColumn.columnIndex == 1)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[1] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[1] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[1])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }

        }
        else if (ParentColumn.columnIndex == 2)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[2] || indexInColumn == ColumnManager.instance.randompayline2.rowIndex[2] || indexInColumn == ColumnManager.instance.randompayline3.rowIndex[2])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }
        }
        else if (ParentColumn.columnIndex == 3)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[3])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }

        }
        else if (ParentColumn.columnIndex == 4)
        {
            if (indexInColumn == ColumnManager.instance.randompayline.rowIndex[4])
            {
                animationIndex = ColumnManager.instance.hightitem;
                itemType = SlotItemType.Normal;
            }
        }
    }

    internal void SetNewAnim()
    {
        Vector3 newscale = InitialScale;
        gameObject.transform.localScale = newscale;


        animationIndex = Random.Range(0, 9);

        if (animationIndex == 9 && transform.parent.transform.name == "Column 1")
        {
            animationIndex = Random.Range(0, 9);
        }


        if (animationIndex < 9)
            itemType = SlotItemType.Normal;
        else if (animationIndex == 9)
            itemType = SlotItemType.Wild;

        else if (animationIndex == 10)
            itemType = SlotItemType.Scatter;
        else
            itemType = SlotItemType.Bonus;

        if (ParentColumn.IsColumnWild)
        {

            itemType = SlotItemType.Wild;
            animationIndex = 9;
        }

        ApplyCurrentVisual();
        once = false;
    }

    private void ApplyCurrentVisual()
    {
        // TryApplyBlurrySprite();

        if (TryApplyBlurrySprite())
            return;

        if (slotItemTextures != null && animationIndex >= 0 && animationIndex < slotItemTextures.Length)
            ApplyTexture(slotItemTextures[animationIndex]);
    }

    private bool TryApplyBlurrySprite()
    {
        if (slotItemBlurrySprites == null || animationIndex < 0 || animationIndex >= slotItemBlurrySprites.Length)
            return false;

        Sprite sprite = slotItemBlurrySprites[animationIndex];
        if (sprite == null)
            return false;

        Material material = GetComponent<Renderer>().material;
        Rect rect = sprite.textureRect;
        material.mainTexture = sprite.texture;
        material.SetTextureScale("_MainTex", new Vector2(rect.width / sprite.texture.width, rect.height / sprite.texture.height));
        material.SetTextureOffset("_MainTex", new Vector2(rect.x / sprite.texture.width, rect.y / sprite.texture.height));
        spriteVisualApplied = true;
        return true;
    }

    private void ApplyTexture(Texture2D texture)
    {
        Material material = GetComponent<Renderer>().material;
        material.mainTexture = texture;
        if (spriteVisualApplied)
        {
            material.SetTextureScale("_MainTex", originalTextureScale);
            material.SetTextureOffset("_MainTex", originalTextureOffset);
            spriteVisualApplied = false;
        }
    }
}// End Script..
