using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveCell : MonoBehaviour, ICell
{
    //UI
    public Text Id;
    public Text Date;
    public Text Class;
    public Text Score;
    public Text Time;
    //Model
    private Save _contactInfo;
    private int _cellIndex;
    //This is called from the SetCell method in DataSource
    public void ConfigureCell(Save saveinfo, int cellIndex)
    {
        _cellIndex = cellIndex;
        _contactInfo = saveinfo;
        Id.text = saveinfo.Path;
        Date.text = saveinfo.Path;
        Class.text = saveinfo.Path;
        Score.text = saveinfo.Path;
        Time.text = saveinfo.Path;
    }


}
