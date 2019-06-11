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
using UnityEngine.EventSystems;

public class RayText : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray3DForOne();
        }
    }
    /// <summary>
    /// 3D射线单体
    /// </summary>
    public void Ray3DForOne()
    {
        //防止点UI时候3D射线穿透误操作
#if UNITY_ANDROID || UNITY_IPHONE
if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
        if (EventSystem.current.IsPointerOverGameObject())
#endif
            return;

        //把鼠标坐标转换为相机位置的一个struct 可理解为转换为当前相机的一个点
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //位运算 获得层级 不过最多只能设置31个， | 后可以加多层 ~取反不照射某一层
        int layer = 1 << LayerMask.NameToLayer("RayLayer");
        RaycastHit hit;
        //射线的起点  碰撞体(需要加碰撞) 射线长度 层级   还有个参数是枚举选择是否报告触发器
        if (Physics.Raycast(ray, out hit, 100, layer))
        {
            //根据Tag再次判定 不然可能layer判定会出一定的错误
            if (hit.transform.CompareTag("cube"))
            {
                Debug.Log("点击到了");
            }
        }
    }

    public void Ray3DForAll()
    {
#if UNITY_ANDROID || UNITY_IPHONE
if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
        if (EventSystem.current.IsPointerOverGameObject())
#endif
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //打开这两个层
        int layer = (1 << LayerMask.NameToLayer("RayLayer")) | (1 << LayerMask.NameToLayer("RayLayer1"));
        RaycastHit[] hit = Physics.RaycastAll(ray, 100, layer);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.CompareTag(""))
                {
                    Debug.Log("找到了");
                }
            }
        }
    }

    public void Ray2DForOne()
    {
        //碰撞器要挂2D的
        //参数为：起点坐标，方向向量 用于人物朝向 墙体判断之类的 检测 不适合点击检测
        //Ray2D ray = new Ray2D(transform.position, Vector2.right);
        //起点坐标 和方向 还有长度 没有层级概念
        //Vector2 mousePos2D = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        //这样就有了层级概念
        int layer = 1 << LayerMask.NameToLayer("RayLayer");
        Collider2D hit=    Physics2D.OverlapPoint(Input.mousePosition,layer);
        //Physics2D.OverlapPointAll();当前鼠标点击下多个碰撞
        if (hit.transform != null && hit.transform.CompareTag("cube"))
        {
            Debug.Log("找到了");
        }
    }

    public void Ray2DForAll()
    {

        //参数为：起点坐标，方向向量 用于任务检测 不适合点击检测
        //Ray2D ray = new Ray2D(transform.position, Vector2.right);
        //起点坐标 和方向 还有长度 层级
        int layer = 1 << LayerMask.NameToLayer("RayLayer");
        Vector2 mousePos2D = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 10,layer);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[0].transform.CompareTag("cube"))
                {
                    Debug.Log("找到了");
                }
            }
        }
    }

    public void Ray3DForSphere()
    {
        //位运算 获得层级 不过最多只能设置31个， | 后可以加多层 ~取反不照射某一层
        int layer = 1 << LayerMask.NameToLayer("RayLayer");
        //射线中心点(就是球的中心点) 半径 层级 一般技能用这个
        Collider[] cols = Physics.OverlapSphere(transform.position, 10, layer);
        //Physics.CheckSphere(transform.position, 10, layer);检测是否有东西
        //Cube检测 中心点 一个宽度 一个旋转(默认不旋转) 层级
       // Collider[] cols1 = Physics.OverlapBox(transform.position,transform.localScale/2,Quaternion.identity,layer);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag(""))
            {

            }
        }
    }

    /// <summary>
    /// 圆形检测
    /// </summary>
    public void Ray2DForCircle()
    {
        //位运算 获得层级 不过最多只能设置31个， | 后可以加多层 ~取反不照射某一层
        int layer = 1 << LayerMask.NameToLayer("RayLayer");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position,10,layer);
        //Box 检测 第三个是角度 
        Collider2D[] cols1 = Physics2D.OverlapBoxAll(transform.position, transform.localScale/2,0 ,layer); 
        if (cols.Length>0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag(""))
                {

                }
            }
        }
    }
}
