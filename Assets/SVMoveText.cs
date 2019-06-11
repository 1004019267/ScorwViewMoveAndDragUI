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

public class SVMoveText : MonoBehaviour
{
    public Transform sv;
    public Transform btn;

    public Transform sv1;
    public Transform btn1;
    // Use this for initialization
    void Start()
    {
        SVMove svMove = new SVMove(sv,4,ESVMoveType.Vertical);
        svMove.correctionHeadCallBack = () =>
        {
            btn.transform.eulerAngles = Vector3.forward * 90;
        };
        svMove.correctionTailCallBack = () =>
        {
            btn.transform.eulerAngles = Vector3.forward * 270;
        };
        btn.GetComponent<Button>().onClick.AddListener(() => { svMove.Move(); });

        SVMove svMove1 = new SVMove(sv1,5, ESVMoveType.Horizontal);
        svMove1.correctionHeadCallBack = () =>
        {
            btn1.transform.eulerAngles = Vector3.forward* 0;
        };
        svMove1.correctionTailCallBack = () =>
        {
            btn1.transform.eulerAngles = Vector3.forward * 180;
        };
        btn1.GetComponent<Button>().onClick.AddListener(() => { svMove1.Move(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
