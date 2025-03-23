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

    private GameObject _entityView;
    private bool _isVisibility = false;
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
    
    public void SetEntityView(eCellType type)
    {
        switch (type)
        {
            case eCellType.None:
                if(_entityView != null)
                    Destroy(_entityView);
                break;
            case eCellType.Black:
                GameObject blackEntity = Resources.Load<GameObject>("BlackEntity");
                var obj =  Instantiate(blackEntity,transform);
                _entityView = obj;
                break;
        }
    }

    private void SetVisibilityView()
    {
        if (_visibilityCount > 0)
        {
            if (!_isVisibility)
            {
                body.color = Color.white;
                _isVisibility = true;
            }
        }
        else
        {
            body.color = Color.gray;
            _isVisibility = false;
        }
    }
}
