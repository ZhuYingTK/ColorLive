using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public partial class BoardTile
{
    public TMP_Text liveText;
    public SpriteRenderer body;
    public Image generateImg;
    public Image produceImg;
    public Image dieImg;
    public void RefreshTileView()
    {
        if (cellEntity == null)
        {
            liveText.gameObject.SetActive(false);
            generateImg.gameObject.SetActive(false);
            produceImg.gameObject.SetActive(false);
            dieImg.gameObject.SetActive(false);
        }
        else
        {
            SetGenerateProgress(cellEntity.generateProgress);
            SetProduceProgress(cellEntity.productProgress);
            SetDieProgress(cellEntity.dieProgress);
            SetBody(cellEntity.GetCellType());
        }
    }
    
    public void SetGenerateProgress(float progress)
    {
        generateImg.fillAmount = progress;
        generateImg.gameObject.SetActive(progress != 1 && progress != 0);
    }
    public void SetProduceProgress(float progress)
    {
        produceImg.fillAmount = progress;
        produceImg.gameObject.SetActive(true);
    }
    public void SetDieProgress(float progress)
    {
        dieImg.fillAmount = progress;
        dieImg.gameObject.SetActive(progress != 1 && progress != 0);
    }
    
    private void SetLiveText()
    {
        liveText.text = cellEntity.data.live.ToString();
        liveText.gameObject.SetActive(true);
    }
    
    public void SetBody(eCellType type)
    {
        switch (type)
        {
            case eCellType.None:
                body.color = Color.white;
                break;
            case eCellType.Black:
                body.color = Color.black;
                break;
        }
    }
}
