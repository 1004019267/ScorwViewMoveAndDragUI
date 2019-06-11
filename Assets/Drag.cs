/**
 *Copyright(C) 2019 by #COMPANY#
 *All rights reserved.
 *FileName:     #SCRIPTFULLNAME#
 *Author:       #AUTHOR#
 *Version:      #VERSION#
 *UnityVersion：#UNITYVERSION#
 *Date:         #DATE#
 *Description:   
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Drag : MonoBehaviour
{
    bool canTouch=true;
    private void Start()
    {
        RegisterDrag(transform);
        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("点击了");        
        });
    }

    /// <summary>
    /// 限制移动距离在屏幕内
    /// </summary>
    /// <param name="tra"></param>
    public void DragRangeLimit(Transform tra)
    {
        var pos = tra.GetComponent<RectTransform>();
        float x = Mathf.Clamp(pos.position.x, pos.rect.width * 0.5f, Screen.width - (pos.rect.width * 0.5f));
        float y = Mathf.Clamp(pos.position.y, pos.rect.height * 0.5f, Screen.height - (pos.rect.height * 0.5f));
        pos.position = new Vector2(x, y);
    }

    public void RegisterDrag(Transform tra)
    {
        if (tra.transform != null)
        {
            var listener = EventTriggerListener.Get(tra.transform);
            Vector3 offset = Vector3.zero;
            Button btn = tra.transform.GetComponent<Button>();
            listener.onBeginDrag += (go) =>
            {
                if (!canTouch)
                    return;
                if (btn != null)
                {
                    btn.enabled = false;
                }
                offset = tra.transform.position - Input.mousePosition;
            };
            listener.onDrag += (go) =>
            {
                if (!canTouch)
                    return;
                tra.transform.position = Input.mousePosition + offset;
                DragRangeLimit(tra.transform);
            };
            listener.onEndDrag += (go) =>
            {
                if (!canTouch)
                    return;
                if (btn != null)
                {
                    btn.enabled = true;
                }
            };
        }
    }
}
