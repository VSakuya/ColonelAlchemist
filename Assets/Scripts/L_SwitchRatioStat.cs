using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class L_SwitchRatioStat : MonoBehaviour {

    public Sprite badSprite;
    public Sprite normalSprite;
    public Sprite goodSprite;
    public Sprite perfectSprite;

    private Image img;

    void Start()
    {
		img = GetComponent<Image>();
       
    }

	public void SwitchSprite(ERatioStat stat)
    {
        switch(stat)
        {
            case ERatioStat.Bad:
				img.sprite = badSprite;
                break;
            case ERatioStat.Normal:
				img.sprite = normalSprite;
                break;
            case ERatioStat.Good:
				img.sprite = goodSprite;
                break;
            case ERatioStat.Perfect:
				img.sprite = perfectSprite;
                break;
        }
    }
}
