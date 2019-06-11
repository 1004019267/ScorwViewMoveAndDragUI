
using System;
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
using DG.Tweening;

public enum ESVMoveType
{
    Horizontal = 0,
    Vertical
}

public class SVMove
{
    /// <summary>
    /// the firstStart do not Use
    /// </summary>
    public Action correctionHeadCallBack;
    public Action correctionTailCallBack;
    public float duration = 1f;
    Vector2 disNormalizedVe2;
    ScrollRect sv;
    GridLayoutGroup grid;
    bool isTail;
    float scorwPercentage;
    int childcount;

    ESVMoveType svType;
    /// <summary>
    /// Set the SV with Btn and Org 
    /// </summary>
    /// <param name="sv">ScrowView</param>
    /// <param name="btn">ControBtn</param>
    /// <param name="org">Orientation</param>
    public SVMove(Transform sv, int scrowNum = 1, ESVMoveType type = ESVMoveType.Vertical)
    {
        this.sv = sv.GetComponent<ScrollRect>();
        grid = this.sv.content.GetComponent<GridLayoutGroup>();
        svType = type;
        childcount = grid.transform.childCount;
        scorwPercentage = (float)scrowNum / childcount;
        GetDistance(childcount);
    }

    /// <summary>
    /// Move Active Response
    /// </summary>
    public void Move()
    {
        float svNormalizedLong = GetSVNormalizedLong();

        if (svNormalizedLong <= 0)
            isTail = false;
        if (svNormalizedLong >= 1)
            isTail = true;

        if (isTail)
            DoForwardMove();
        else
            DoBackwordMove();
    }

    /// <summary>
    /// Forward move
    /// </summary>
    public void DoForwardMove()
    {
        ChangeDisWithChildcount();
        Vector2 willPos = sv.normalizedPosition - disNormalizedVe2;
        Vector2 correctPos = MovieRangeLimit(willPos);
        float correctDur = GetDuarationLimitWithDis(willPos, correctPos);

        DOTween.To(() => sv.normalizedPosition, x => sv.normalizedPosition = x,
            correctPos, correctDur).OnComplete(() =>
            {
                //Correction the tail with End
                if (GetSVNormalizedLong() <= 0.01)
                {
                    if (correctionTailCallBack != null)
                        correctionTailCallBack();

                    isTail = false;
                }
            });
    }

    /// <summary>
    /// Backword Move
    /// </summary>
    public void DoBackwordMove()
    {
        ChangeDisWithChildcount();
        Vector2 willPos = sv.normalizedPosition + disNormalizedVe2;
        Vector2 correctPos = MovieRangeLimit(willPos);
        float correctDur = GetDuarationLimitWithDis(willPos, correctPos);

        DOTween.To(() => sv.normalizedPosition, x => sv.normalizedPosition = x,
            correctPos, correctDur).OnComplete(() =>
            {
                //Correction the head  with Start
                if (GetSVNormalizedLong() >= 0.99f)
                {
                    if (correctionHeadCallBack != null)
                        correctionHeadCallBack();

                    isTail = true;
                }
            });
    }
    /// <summary>
    /// if childcount change the moveDistance will be reCalculation
    /// </summary>
    void ChangeDisWithChildcount()
    {
        if (childcount != grid.transform.childCount)
        {
            childcount = grid.transform.childCount;
            GetDistance(childcount);
        }
    }
    /// <summary>
    /// Get the Distance with ScrowNum and Orientation
    /// </summary>
    /// <param name="scrowNum"></param>
    /// <param name="org"></param>
    void GetDistance(int childcount)
    {
        switch (svType)
        {
            case ESVMoveType.Horizontal:
                float childWidth = grid.cellSize.x;
                float maxContentWidth = (childWidth + grid.spacing.x) * childcount - grid.padding.top - sv.GetComponent<RectTransform>().rect.width;
                float disWidth = (childWidth + grid.spacing.x) * scorwPercentage * childcount / maxContentWidth;
                disNormalizedVe2 = disWidth * Vector2.right;
                break;
            case ESVMoveType.Vertical:
                float childHeight = grid.cellSize.y;
                float maxContentHeight = (childHeight + grid.spacing.y) * childcount - grid.padding.left - sv.GetComponent<RectTransform>().rect.height;
                float disHeight = (childHeight + grid.spacing.y) * scorwPercentage * childcount / maxContentHeight;
                disNormalizedVe2 = disHeight * Vector2.up;
                break;
        }
    }
    /// <summary>
    /// chang the Orientation if you want  Backword start 
    /// </summary>
    public void ChangeOrientation()
    {
        switch (svType)
        {
            case ESVMoveType.Horizontal:
                disNormalizedVe2.x *= -1;
                break;
            case ESVMoveType.Vertical:
                disNormalizedVe2.y *= -1;
                break;
        }
    }
    /// <summary>
    /// get the SVNormalizedLong with the type
    /// </summary>
    /// <param name="norLong"></param>
    float GetSVNormalizedLong()
    {
        switch (svType)
        {
            case ESVMoveType.Horizontal:
                return sv.normalizedPosition.x;

            case ESVMoveType.Vertical:
                return sv.normalizedPosition.y;
        }
        return 0;
    }

    //get WillPos limit with movie
    Vector2 MovieRangeLimit(Vector2 willPos)
    {
        switch (svType)
        {
            case ESVMoveType.Horizontal:
                if (willPos.x > 1)
                    willPos.x = 1;
                else if (willPos.x < 0)
                    willPos.x = 0;
                break;
            case ESVMoveType.Vertical:
                if (willPos.y > 1)
                    willPos.y = 1;
                else if (willPos.y < 0)
                    willPos.y = 0;
                break;
        }
        return willPos;
    }

    //limitd the Duaration with WillPos and CorrectPos,it can let movie become more smooth
    float GetDuarationLimitWithDis(Vector2 willPos, Vector2 correctPos)
    {
        if (willPos == correctPos)
            return duration;

        switch (svType)
        {
            case ESVMoveType.Horizontal:
                return (willPos.x - correctPos.x) / (willPos.x < 0 ? willPos.x - 1 : willPos.x) * duration;
            case ESVMoveType.Vertical:
                return (willPos.y - correctPos.y) / (willPos.y < 0 ? willPos.y - 1 : willPos.y) * duration;
        }
        return duration;
    }
}
