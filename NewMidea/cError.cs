using System;
using System.Collections.Generic;
using System.Text;

namespace NewMideaProgram
{
    /// <summary>
    /// 错误信息
    /// </summary>
    public class ErrDataInfo
    {
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isShow;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string data;
        /// <summary>
        /// 错误信息类型
        /// </summary>
        public string errDataEnum;
        public void SetErrorStatue(bool statue)
        {
            isShow = statue;
        }
    }
    public enum ErrDataEnum
    {
        Info,
        Error,
        Protect
    }
    public class cError
    {
        /// <summary>
        /// 错误信息表
        /// </summary>
        List<ErrDataInfo> flushErrDataInfo = new List<ErrDataInfo>();//主要作用是把错误信息分开显示,以免造成不同的信息提示间乱刷新的问题
        /// <summary>
        /// 当前错误信息号
        /// </summary>
        int  NowShowErrIndex =0;
        object o = new object();
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="name">string,错误信息名,用于复位时</param>
        /// <param name="ErrorDescribe">string,错误信息的描述</param>
        public void AddErrData(string name,string ErrorDescribe)
        {
            for (int i = 0; i < flushErrDataInfo.Count; i++)
            {
                if (flushErrDataInfo[i].errDataEnum == name)//以包含这个错误
                {
                    flushErrDataInfo[i].data = ErrorDescribe;
                    return;
                }
            }
            ErrDataInfo errDataInfo = new ErrDataInfo();
            errDataInfo.isShow = true;
            errDataInfo.errDataEnum = name;
            errDataInfo.data = ErrorDescribe;
            lock (o)
            {
                flushErrDataInfo.Add(errDataInfo);
            }
        }
        public void DelErrData(string name)
        {
            for (int i = 0; i < flushErrDataInfo.Count; i++)
            {
                if (flushErrDataInfo[i].errDataEnum == name)
                {
                    lock (o)
                    {
                        flushErrDataInfo.Remove(flushErrDataInfo[i]);
                    }
                }
            }
        }
        public string GetError()
        {
            string s = "";
            lock (o)
            {
                NowShowErrIndex++;
                if (flushErrDataInfo != null)
                {
                    if (flushErrDataInfo.Count > 0)
                    {
                        NowShowErrIndex = NowShowErrIndex % flushErrDataInfo.Count;
                        s = flushErrDataInfo[NowShowErrIndex].data;
                    }
                }
                
            }
            return s;
        }
        public ErrDataEnum GetErrEnum()
        {
            ErrDataEnum errDataEnum = ErrDataEnum.Info;
            if (flushErrDataInfo != null)
            {
                if (flushErrDataInfo.Count > 0)
                {
                    if (flushErrDataInfo[NowShowErrIndex].errDataEnum.IndexOf("ERROR") >= 0)
                    {
                        errDataEnum = ErrDataEnum.Error;
                    }
                    if (flushErrDataInfo[NowShowErrIndex].errDataEnum.IndexOf("PROTECT") >= 0)
                    {
                        errDataEnum = ErrDataEnum.Protect;
                    }
                }
            }
            return errDataEnum;
        }
    }
}
