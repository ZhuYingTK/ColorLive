using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour
{
    public ColorEntity _colorEntity;
    public TMP_Text liveText;
    public Image bodyImg;
    public Image generateImg;
    public Image produceImg;
    public Image dieImg;
    public Vector2Int pos;
    public Button btn;
    
    public void Start()
    {
        btn.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if (_colorEntity != null)
        {
            _colorEntity.OnUpdate(this, Time.deltaTime);
        }
    }

    //点击事件
    private void OnClick()
    {
        if (_colorEntity == null || _colorEntity.data.status == eEntityStatus.Generating)
        {
            if (GameManager.Instance.blackRes >= BlackEntity.cost)
            {
                _colorEntity = new BlackEntity(status:eEntityStatus.Stable);
                GameManager.Instance.blackRes -= BlackEntity.cost;
                liveText.text = _colorEntity.data.live.ToString();
                setLiveText();
                RefreshTileView();
            }
        }
    }

    public void Generate(eCellType type,int live)
    {
        if(_colorEntity != null && _colorEntity.data.status == eEntityStatus.Generating) return;
        switch (type)
        {
            case eCellType.Black:
                _colorEntity = new BlackEntity(live,status: eEntityStatus.Generating);
                _colorEntity.OnStartGenerate(this);
                setLiveText();
                break;
        }
    }

    public void StartDie()
    {
        _colorEntity.OnStartDie(this);
    }

    public void StopDie()
    {
        _colorEntity.OnStopDie(this);
    }

    public void StopGenerate()
    {
        _colorEntity.OnStopGenerate(this);
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

    public void EntityDead()
    {
        _colorEntity = null;
        SetBody(eCellType.None);
        RefreshTileView();
    }

    public void RefreshTileView()
    {
        if (_colorEntity == null)
        {
            liveText.gameObject.SetActive(false);
            generateImg.gameObject.SetActive(false);
            produceImg.gameObject.SetActive(false);
            dieImg.gameObject.SetActive(false);
        }
        else
        {
            SetGenerateProgress(_colorEntity.generateProgress);
            SetProduceProgress(_colorEntity.productProgress);
            SetDieProgress(_colorEntity.dieProgress);
            SetBody(_colorEntity.GetColorType());
        }
    }

    public void SetBody(eCellType type)
    {
        switch (type)
        {
            case eCellType.None:
                bodyImg.color = Color.white;
                break;
            case eCellType.Black:
                bodyImg.color = Color.black;
                break;
        }
    }

    public eCellType GetColorType()
    {
        if (_colorEntity == null)
            return eCellType.None;
        else
            return _colorEntity.GetColorType();
    }

    public eEntityStatus GetColorStatus()
    {
        if (_colorEntity == null)
            return eEntityStatus.None;
        else
            return _colorEntity.data.status;
    }

    public int? GetColorLive()
    {
        if (_colorEntity == null || _colorEntity.data.status == eEntityStatus.Generating)
            return null;
        else
            return _colorEntity.data.live;
    }

    private void setLiveText()
    {
        liveText.text = _colorEntity.data.live.ToString();
        liveText.gameObject.SetActive(true);
    }
}
